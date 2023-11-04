using API.Accounts.Application.DTOs.ExternaDTOs;
using API.Accounts.Application.DTOs.ExternalDTOs;
using API.Accounts.Application.Services.HttpService;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService
{
    public class StockActionManager : IStockActionManager
    {
        private readonly IHttpService _httpService;

        public StockActionManager(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            foreach (var stock in stocks)
            {
                finalizeDto.Stocks.Add(new StockActionInfo()
                {
                    Quantity = stock.WaitingForPurchaseCount,
                    SinglePrice = await GetStockPrice(stock.StockName),
                    StockId = stock.Id
                });
            }

            await _httpService.PostAsync("url...", finalizeDto);
        }

        public Task ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks)
        {
            throw new NotImplementedException();
        }

        private async Task<decimal> GetStockPrice(string name)
        {
            var stockApiResponse = await _httpService
                .GetAsync<StockApiResponseDTO>($"https://localhost:7160/api/Stock/Current/{name}");

            return (decimal)stockApiResponse.Close;
        }
    }
}
