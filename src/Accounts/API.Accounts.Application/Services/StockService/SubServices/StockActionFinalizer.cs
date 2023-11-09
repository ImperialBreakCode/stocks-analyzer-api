using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    public class StockActionFinalizer : IStockActionFinalizer
    {
        private readonly IAccountsData _accountsData;
        private readonly IStockActionExecuter _actionExecuter;

        public StockActionFinalizer(IAccountsData accountsData, IStockActionExecuter actionExecuter)
        {
            _accountsData = accountsData;
            _actionExecuter = actionExecuter;
        }

        public async Task<string> ConfirmPurchase(string walletId)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(walletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                var stocksForPurchase = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForPurchaseCount != 0);

                var finalizeDto = CreateFinalizeStockDto(false, wallet.Id, wallet.UserId);

                var res = await _actionExecuter.ExecutePurchase(finalizeDto, stocksForPurchase);

                ReflectConfirmationChanges(res, stocksForPurchase, context);

                wallet.Balance -= res.TotalSuccessfulPrice;
                context.Wallets.Update(wallet);

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        public async Task<string> ConfirmSales(string walletId)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(walletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                var stocksForSale = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForSaleCount != 0);

                var finalizeDto = CreateFinalizeStockDto(true, wallet.Id, wallet.UserId);

                var res = await _actionExecuter.ExecuteSell(finalizeDto, stocksForSale);

                ReflectConfirmationChanges(res, stocksForSale, context);

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        private FinalizeStockActionDTO CreateFinalizeStockDto(bool forSale, string walletId, string userId)
        {
            FinalizeStockActionDTO finalizeDto = new FinalizeStockActionDTO();
            finalizeDto.IsSale = forSale;
            finalizeDto.WalletId = walletId;
            finalizeDto.UserId = userId;

            return finalizeDto;
        }

        private void ReflectConfirmationChanges(FinalizeStockResponseDTO res, ICollection<Stock> currentStocks, IAccountsDbContext context)
        {
            foreach (var responseStock in res.StockInfoResponseDTOs)
            {
                if (responseStock.IsSuccessful)
                {
                    Stock stock = currentStocks.Where(s => s.Id == responseStock.StockId).First();

                    if (res.IsSale)
                    {
                        stock.Quantity -= stock.WaitingForSaleCount;
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
}
