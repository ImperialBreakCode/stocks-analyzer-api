using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    public class StockActionManager : IStockActionManager
    {
        private readonly IHttpService _httpService;
        private readonly IHttpClientRoutes _httpClientRoutes;
        private readonly IAccountsData _accountsData;

        public StockActionManager(IHttpService httpService, IHttpClientRoutes httpClientRoutes, IAccountsData accountsData)
        {
            _httpService = httpService;
            _httpClientRoutes = httpClientRoutes;
            _accountsData = accountsData;
        }

        public async Task<string> AddForPurchase(StockActionDTO stockActionDTO)
        {
            var stockApiResponse = await _httpService
                .GetAsync<StockApiResponseDTO>(_httpClientRoutes.GetCurrentStockInfoRoute(stockActionDTO.StockName));

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
                context.Stocks.Update(stock);

                context.Commit();
            }

            return String.Format(ResponseMessages.StockActionSuccessfull, "sale");
        }
    }
}
