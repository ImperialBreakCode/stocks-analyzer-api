using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.ExternaDTOs;
using API.Accounts.Application.DTOs.ExternalDTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.HttpService;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService
{
    public class StockService : IStockService
    {
        private readonly IAccountsData _accountsData;
        private readonly IHttpService _httpService;
        private readonly IStockActionManager _stockActionManager;

        public StockService(IAccountsData accountsData, IHttpService httpService, IStockActionManager stockActionManager)
        {
            _accountsData = accountsData;
            _httpService = httpService;
            _stockActionManager = stockActionManager;
        }

        public async Task<string> AddForPurchase(StockActionDTO stockActionDTO)
        {
            var stockApiResponse = await _httpService
                .GetAsync<StockApiResponseDTO>($"https://localhost:7160/api/Stock/Current/{stockActionDTO.StockName}");

            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(stockActionDTO.WalletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                if (wallet.Balance < (decimal)stockApiResponse!.Close * stockActionDTO.Quantity)
                {
                    return ResponseMessages.NotEnoughBalance;
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

        public string AddForSale(StockActionDTO stockActionDTO)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(stockActionDTO.WalletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                Stock? stock = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.StockName == stockActionDTO.StockName)
                    .FirstOrDefault();

                if (stock is null)
                {
                    return ResponseMessages.StockNotFoundInWallet;
                }

                if (stock.Quantity < stockActionDTO.Quantity)
                {
                    return ResponseMessages.StockNotEnoughStocksToSale;
                }

                stock.WaitingForSaleCount += stockActionDTO.Quantity;
                stock.Quantity -= stockActionDTO.Quantity;
                context.Stocks.Update(stock);

                context.Commit();
            }

            return String.Format(ResponseMessages.StockActionSuccessfull, "sale");
        }
        
        public async Task<string> ConfirmPurchase(string walletId)
        {
            FinalizeStockActionDTO finalizeDto = new FinalizeStockActionDTO();
            finalizeDto.IsSale = false;

            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(walletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }

                finalizeDto.WalletId = wallet.Id;
                finalizeDto.UserId = wallet.UserId;
                finalizeDto.Stocks = new List<StockActionInfo>();

                var stocks = context.Stocks
                    .GetManyByCondition(s => s.WalletId == wallet.Id && s.WaitingForPurchaseCount != 0);

                await _stockActionManager.ExecutePurchase(finalizeDto, stocks);

                foreach (var stock in stocks)
                {
                    stock.WaitingForPurchaseCount = 0;
                    context.Stocks.Update(stock);
                }

                context.Commit();
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        public string ConfirmSales(string walletId)
        {
            throw new NotImplementedException();
        }
    }
}
