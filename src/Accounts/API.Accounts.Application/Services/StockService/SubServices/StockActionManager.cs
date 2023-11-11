using API.Accounts.Application.Data;
using API.Accounts.Application.Data.StocksData;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    public class StockActionManager : IStockActionManager
    {
        private readonly IAccountsData _accountsData;
        private readonly IStocksData _stocksData;

        public StockActionManager(IAccountsData accountsData, IStocksData stocksData)
        {
            _accountsData = accountsData;
            _stocksData = stocksData;
        }

        public async Task<string> AddForPurchase(StockActionDTO stockActionDTO)
        {
            decimal stockPrice = await _stocksData.GetCurrentStockPrice(stockActionDTO.StockName);

            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(stockActionDTO.WalletId);
                string? errorMessage = await CheckWallet(wallet, false, stockActionDTO);

                if (errorMessage is not null)
                {
                    return errorMessage;
                }

                var stock = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.StockName == stockActionDTO.StockName)
                    .FirstOrDefault();

                if (stock is null)
                {
                    stock = new Stock()
                    {
                        StockName = stockActionDTO.StockName,
                        WalletId = wallet.Id,
                    };

                    context.Stocks.Insert(stock);
                }

                stock.WaitingForPurchaseCount += stockActionDTO.Quantity;
                context.Stocks.Update(stock);

                context.Commit();
            }

            return String.Format(ResponseMessages.StockActionSuccessfull, "purchase");
        }

        public async Task<string> AddForSale(StockActionDTO stockActionDTO)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(stockActionDTO.WalletId);
                string? errorMessage = await CheckWallet(wallet, true, stockActionDTO);

                if (errorMessage is not null)
                {
                    return errorMessage;
                }

                Stock? stock = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.StockName == stockActionDTO.StockName)
                    .FirstOrDefault();

                if (stock is null)
                {
                    return ResponseMessages.StockNotFoundInWallet;
                }
                else if (stock.Quantity < stockActionDTO.Quantity)
                {
                    return ResponseMessages.StockNotEnoughStocksToSale;
                }

                stock.WaitingForSaleCount += stockActionDTO.Quantity;
                context.Stocks.Update(stock);

                context.Commit();
            }

            return String.Format(ResponseMessages.StockActionSuccessfull, "sale");
        }

        private async Task<string?> CheckWallet(Wallet? wallet, bool isSale, StockActionDTO stockActionDTO)
        {
            if (wallet is null)
            {
                return ResponseMessages.WalletNotFound;
            }
            else if ((DateTime.UtcNow - wallet.CreatedAt).Days > 30)
            {
                return ResponseMessages.WalletRestricted;
            }

            if (!isSale)
            {
                decimal stockPrice = await _stocksData.GetCurrentStockPrice(stockActionDTO.StockName);

                if (wallet.Balance < stockPrice * stockActionDTO.Quantity)
                {
                    return ResponseMessages.NotEnoughBalance;
                }
            }

            return null;
        }
    }
}
