using Newtonsoft.Json;
using API.Analyzer.Domain.Interfaces;
using API.StockAPI.Domain.Models;
using API.Accounts.Application.DTOs.Response;

namespace API.Analyzer.Infrastructure.Services
{
    public class ApiService : IApiService 
    {

        private readonly HttpClient accountsClient;
        private readonly HttpClient stocksClient;

        public ApiService()
        {
            accountsClient = new HttpClient();
            accountsClient.BaseAddress = new Uri("https://localhost:5032");

            stocksClient = new HttpClient();
            stocksClient.BaseAddress = new Uri("https://localhost:5031");
        }

        // Connection with AccountsAPI
        public async Task<GetWalletResponseDTO> PortfolioSummary(string walletId)
        {
            string getUrl = $"/api/Wallet/GetWallet/{walletId}";

            HttpResponseMessage response = await accountsClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GetWalletResponseDTO>(jsonContent);
                return result;

            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }

        }

        public async Task<decimal> CurrentProfitability(string walletId)
        {
            string getProfitabilityUrl = $"/api/Wallet/GetWalletBalance/{walletId}";
            HttpResponseMessage response = await accountsClient.GetAsync(getProfitabilityUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var walletResponse = JsonConvert.DeserializeObject<decimal>(jsonContent);

                return walletResponse;
            }

            Console.WriteLine($"HTTP Error: {response.StatusCode}");
            throw new Exception($"HTTP Error: {response.StatusCode}");
        }

        //Connection with StockAPI
        public async Task<StockData> GetStockInfo(string symbol)
        {
            string getUrl = $"/api/stocks/{symbol}";

            HttpResponseMessage response = await stocksClient.GetAsync(getUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<StockData>(jsonContent);
                return result;
            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }
        }

        //public async Task<decimal> PercentageChange(string symbol)
        //{
        //    try
        //    {
        //        StockData stockResponse = await GetStockInfo(symbol);

        //        decimal currentNetIncome = stockResponse.NetIncome;
        //        decimal currentCommonSharesOutstanding = stockResponse.CommonSharesOutstanding;

        //        // Calculate Percentage Change
        //        decimal percentageChange = (currentNetIncome / currentCommonSharesOutstanding) * 100;

        //        return percentageChange;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error retrieving current stock percentage change: {ex.Message}");
        //        throw;
        //    }
        //}

    }

}

