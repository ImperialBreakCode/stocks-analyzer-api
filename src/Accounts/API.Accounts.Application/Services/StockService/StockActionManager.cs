using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.Services.HttpService;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService
{
    public class StockActionManager : IStockActionManager
    {
        private readonly IHttpService _httpService;
        private readonly IHttpClientRoutes _httpRoutes;

        public StockActionManager(IHttpService httpService, IHttpClientRoutes httpRoutes)
        {
            _httpService = httpService;
            _httpRoutes = httpRoutes;
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

        private async Task<FinalizeStockResponseDTO> FinishAction(FinalizeStockActionDTO finalizeDto)
        {
            return await _httpService.PostAsync<FinalizeStockResponseDTO>(_httpRoutes.FinalizeStockActionRoute, finalizeDto);
        }

        private async Task<StockActionInfo> CreateStockActionInfo(int quantity, Stock stock)
        {
            return new StockActionInfo()
            {
                Quantity = quantity,
                SinglePriceExcludingCommission = await GetStockPrice(stock.StockName),
                StockId = stock.Id,
                StockName = stock.StockName
            };
        }

        private async Task<decimal> GetStockPrice(string name)
        {
            var stockApiResponse = await _httpService
                .GetAsync<StockApiResponseDTO>(_httpRoutes.GetCurrentStockInfoRoute(name));

            return (decimal)stockApiResponse.Close;
        }
    }
}
