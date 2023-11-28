using API.Accounts.Application.DTOs.Response;
using API.Analyzer.Domain.Interfaces;
using Newtonsoft.Json;

namespace API.Analyzer.Infrastructure.Services
{
    public class AnalyzerStockService : IAnalyzerStockService
    {

        private readonly HttpClient accountsClient;
        private readonly HttpClient stocksClient;

        public AnalyzerStockService()
        {
            accountsClient = new HttpClient();
            accountsClient.BaseAddress = new Uri("https://localhost:5032");

            stocksClient = new HttpClient();
            stocksClient.BaseAddress = new Uri("https://localhost:5031");
        }

        // Formula for Calculating Percentage Gain or Loss
        // Investment percentage gain  = ((Price sold − purchase price) / purchase price ) × 100
       
        public async Task<ICollection<GetStockResponseDTO>>UserStocksInWallet(string walletId)
        {
            string getUrl = $"/api/Stock/GetStocksInWallet/{walletId}";

            HttpResponseMessage response = await accountsClient.GetAsync(getUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ICollection<GetStockResponseDTO>>(jsonContent);
                return result;
            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }
        }
    }
}
