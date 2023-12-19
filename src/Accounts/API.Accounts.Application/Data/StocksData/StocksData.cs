using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Application.Exceptions;
using API.Accounts.Application.HttpClientService;

namespace API.Accounts.Application.Data.StocksData
{
    public class StocksData : IStocksData
    {
        private readonly IHttpService _httpService;
        private readonly IHttpClientRoutes _httpClientRoutes;

        public StocksData(IHttpService httpService, IHttpClientRoutes httpClientRoutes)
        {
            _httpService = httpService;
            _httpClientRoutes = httpClientRoutes;
        }

        public async Task<decimal> GetCurrentStockPrice(string stockName)
        {
            decimal currentPrice;

            try
            {
                var response = await _httpService.GetAsync<StockApiResponseDTO>(_httpClientRoutes.GetCurrentStockInfoRoute(stockName));
                currentPrice = (decimal)response!.Close;
            }
            catch (HttpRequestException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw new StockAPIConnectionException(ExceptionMessages.StockAPIServiceException, ex);
            }

            return currentPrice;
        }
    }
}
