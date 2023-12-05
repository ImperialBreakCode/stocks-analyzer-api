using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Application.Services.UserService.UserRankService;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    internal class StockActionFinalizer : IStockActionFinalizer
    {
        private readonly IAccountsData _accountsData;
        private readonly IStockActionExecuter _actionExecuter;
        private readonly IUserRankManager _userTypeManager;

        public StockActionFinalizer(IAccountsData accountsData, IStockActionExecuter actionExecuter, IUserRankManager userTypeManager)
        {
            _accountsData = accountsData;
            _actionExecuter = actionExecuter;
            _userTypeManager = userTypeManager;
        }

        public async Task<string> ConfirmPurchase(string username)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                string? error = ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);
                if (error is not null)
                {
                    return error;
                }
                else if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                User user = context.Users.GetOneByUserName(username)!;

                var stocksForPurchase = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForPurchaseCount != 0);

                if (!stocksForPurchase.Any())
                {
                    return ResponseMessages.NoStocksAddedForPurchaseSale;
                }

                var finalizeDto = CreateFinalizeStockDto(false, wallet, user.Email);

                var res = await _actionExecuter.ExecutePurchase(finalizeDto, stocksForPurchase);

                ReflectStockQuantityChanges(res, stocksForPurchase, context);

                wallet.Balance -= res.TotalSuccessfulPrice;
                context.Wallets.Update(wallet);

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        public async Task<string> ConfirmSales(string username)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                string? error = ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);
                if (error is not null)
                {
                    return error;
                }
                else if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                User user = context.Users.GetConfirmedByUserName(username)!;

                var stocksForSale = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForSaleCount != 0);

                if (!stocksForSale.Any())
                {
                    return ResponseMessages.NoStocksAddedForPurchaseSale;
                }

                var finalizeDto = CreateFinalizeStockDto(true, wallet, user.Email);

                var res = await _actionExecuter.ExecuteSell(finalizeDto, stocksForSale);

                ReflectStockQuantityChanges(res, stocksForSale, context);

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        private FinalizeStockActionDTO CreateFinalizeStockDto(bool forSale, Wallet wallet, string userEmail)
        {
            FinalizeStockActionDTO finalizeDto = new FinalizeStockActionDTO();
            finalizeDto.IsSale = forSale;
            finalizeDto.WalletId = wallet.Id;
            finalizeDto.UserId = wallet.UserId;
            finalizeDto.UserRank = (UserRank)_userTypeManager.GetUserType(wallet)!;
            finalizeDto.UserEmail = userEmail;

            return finalizeDto;
        }

        private static void ReflectStockQuantityChanges(FinalizeStockResponseDTO res, ICollection<Stock> currentStocks, IAccountsDbContext context)
        {
            foreach (var responseStock in res.AvailabilityStockInfoResponseDTOs)
            {
                Stock stock = currentStocks.Where(s => s.Id == responseStock.StockId).First();

                if (res.IsSale)
                {
                    if (responseStock.IsSuccessful)
                    {
                        stock.Quantity -= stock.WaitingForSaleCount;
                    }

                    stock.WaitingForSaleCount = 0;
                }
                else
                {
                    stock.WaitingForPurchaseCount = 0;
                }

                context.Stocks.Update(stock);
            }
        }
    }
}
