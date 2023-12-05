using Newtonsoft.Json;
using API.Analyzer.Domain.Interfaces;
using API.Analyzer.Domain.DTOs;

namespace API.Analyzer.Infrastructure.Services
{
    public class AnalyzerUserService : IAnalyzerUserService
    {

        private readonly HttpClient accountsClient;

        public AnalyzerUserService()
        {
            accountsClient = new HttpClient();
            accountsClient.BaseAddress = new Uri("https://localhost:5032");
        }

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
    }

}
