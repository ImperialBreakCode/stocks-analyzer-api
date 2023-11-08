using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.HttpService;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services.StockService
{
    public class StockService : IStockService
    {
        private readonly IAccountsData _accountsData;
        private readonly IHttpService _httpService;
        private readonly IStockActionManager _stockActionManager;
        private readonly IHttpClientRoutes _httpClientRoutes;

        public StockService(
            IAccountsData accountsData, 
            IHttpService httpService, 
            IStockActionManager stockActionManager, 
            IHttpClientRoutes httpClientRoutes)
        {
            _accountsData = accountsData;
            _httpService = httpService;
            _stockActionManager = stockActionManager;
            _httpClientRoutes = httpClientRoutes;
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

                try
                {
                    var res = await _stockActionManager.ExecutePurchase(finalizeDto, stocksForPurchase);
                    ReflectConfirmationChanges(res, stocksForPurchase, context);

                    wallet.Balance -= res.TotalSuccessfulPrice;
                    context.Wallets.Update(wallet);
                    context.Commit();
                }
                catch (HttpRequestException ex)
                {
                    return "Failed";
                }
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

                try
                {
                    var res = await _stockActionManager.ExecuteSell(finalizeDto, stocksForSale);
                    
                    ReflectConfirmationChanges(res, stocksForSale, context);
                    
                    context.Commit();
                }
                catch (HttpRequestException ex)
                {
                    return "Failed";
                }
            }

            return ResponseMessages.TransactionSendForProccessing;
        }

        public GetStockResponseDTO? GetStockById(string stockId)
        {
            GetStockResponseDTO? result = null;

            using (var context = _accountsData.CreateDbContext())
            {
                Stock? stock = context.Stocks.GetOneById(stockId);

                if (stock is not null)
                {
                    result = new GetStockResponseDTO()
                    {
                        StockId = stock.Id,
                        Quantity = stock.Quantity,
                        StockName = stock.StockName,
                        WalletId = stock.WalletId
                    };
                }
            }

            return result;
        }

        public ICollection<GetStockResponseDTO>? GetStocksByWalletId(string walletId)
        {
            ICollection<GetStockResponseDTO>? result = null;

            using (var context = _accountsData.CreateDbContext())
            {
                if (context.Wallets.GetOneById(walletId) is not null)
                {
                    result = new List<GetStockResponseDTO>();

                    var stocks = context.Stocks.GetManyByCondition(s => s.WalletId == walletId);

                    foreach (var stock in stocks)
                    {
                        result.Add(new GetStockResponseDTO()
                        {
                            StockId = stock.Id,
                            StockName = stock.StockName,
                            Quantity = stock.Quantity,
                            WalletId = stock.WalletId
                        });
                    }

                }
                
            }

            return result;
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
