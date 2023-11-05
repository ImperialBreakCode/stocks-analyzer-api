using API.Accounts.Application.DTOs.ExternaDTOs;
using API.Accounts.Application.DTOs.ExternalDTOs;
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

        public async Task ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            finalizeDto.Stocks = stocks
                .Select(s => CreateStockActionInfo(s.WaitingForPurchaseCount, s).Result)
                .ToList();

            await FinishAction(finalizeDto);
        }

        public async Task ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            finalizeDto.Stocks = stocks
                .Select(s => CreateStockActionInfo(s.WaitingForSaleCount, s).Result)
                .ToList();

            await FinishAction(finalizeDto);
        }

        private async Task FinishAction(FinalizeStockActionDTO finalizeDto)
        {
            await _httpService.PostAsync(_httpRoutes.FinalizeStockActionRoute, finalizeDto);
        }

        private async Task<StockActionInfo> CreateStockActionInfo(int quantity, Stock stock)
        {
            return new StockActionInfo()
            {
                Quantity = quantity,
                SinglePrice = await GetStockPrice(stock.StockName),
                StockId = stock.Id
            };
        }

        private async Task<decimal> GetStockPrice(string name)
        {
            var stockApiResponse = await _httpService
                .GetAsync<StockApiResponseDTO>($"https://localhost:7160/api/Stock/Current/{name}");

            return (decimal)stockApiResponse.Close;
        }
    }
}
