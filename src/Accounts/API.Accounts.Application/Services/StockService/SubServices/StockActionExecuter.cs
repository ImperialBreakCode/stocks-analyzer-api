using API.Accounts.Application.Data.StocksData;
using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService.SubServices
{
    internal class StockActionExecuter : IStockActionExecuter
    {
        private readonly IHttpService _httpService;
        private readonly IHttpClientRoutes _httpRoutes;
        private readonly IStocksData _stockData;

        public StockActionExecuter(IHttpService httpService, IHttpClientRoutes httpRoutes, IStocksData stocksData)
        {
            _httpService = httpService;
            _httpRoutes = httpRoutes;
            _stockData = stocksData;
        }

        public async Task<FinalizeStockResponseDTO> ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            finalizeDto.StockInfoRequestDTOs = stocks
                .Select(s => CreateStockActionInfo(s.WaitingForPurchaseCount, s).Result)
                .ToList();

            return await FinishAction(finalizeDto);
        }

        public async Task<FinalizeStockResponseDTO> ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            finalizeDto.StockInfoRequestDTOs = stocks
                .Select(s => CreateStockActionInfo(s.WaitingForSaleCount, s).Result)
                .ToList();

            return await FinishAction(finalizeDto);
        }

        private async Task<StockActionInfo> CreateStockActionInfo(int quantity, Stock stock)
        {
            return new StockActionInfo()
            {
                Quantity = quantity,
                SinglePriceExcludingCommission = await _stockData.GetCurrentStockPrice(stock.StockName),
                StockId = stock.Id,
                StockName = stock.StockName
            };
        }

        private async Task<FinalizeStockResponseDTO> FinishAction(FinalizeStockActionDTO finalizeDto)
        {
            return await _httpService
                .PostAsync<FinalizeStockResponseDTO>(_httpRoutes.FinalizeStockActionRoute, finalizeDto);
        }
    }
}
