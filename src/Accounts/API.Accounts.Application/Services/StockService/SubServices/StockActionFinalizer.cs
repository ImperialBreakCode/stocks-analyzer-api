using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    public class StockActionFinalizer : IStockActionFinalizer
    {
        private readonly IAccountsData _accountsData;
        private readonly IStockActionExecuter _actionExecuter;
        private readonly IUserTypeManager _userTypeManager;

        public StockActionFinalizer(IAccountsData accountsData, IStockActionExecuter actionExecuter, IUserTypeManager userTypeManager)
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

                var stocksForPurchase = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForPurchaseCount != 0);

                if (!stocksForPurchase.Any())
                {
                    return ResponseMessages.NoStocksAddedForPurchaseSale;
                }

                var finalizeDto = CreateFinalizeStockDto(false, wallet);

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

                var stocksForSale = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForSaleCount != 0);

                if (!stocksForSale.Any())
                {
                    return ResponseMessages.NoStocksAddedForPurchaseSale;
                }

                var finalizeDto = CreateFinalizeStockDto(true, wallet);

                var res = await _actionExecuter.ExecuteSell(finalizeDto, stocksForSale);

                ReflectStockQuantityChanges(res, stocksForSale, context);

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        private FinalizeStockActionDTO CreateFinalizeStockDto(bool forSale, Wallet wallet)
        {
            FinalizeStockActionDTO finalizeDto = new FinalizeStockActionDTO();
            finalizeDto.IsSale = forSale;
            finalizeDto.WalletId = wallet.Id;
            finalizeDto.UserId = wallet.UserId;
            finalizeDto.UserRank = (UserType)_userTypeManager.GetUserType(wallet)!;

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
