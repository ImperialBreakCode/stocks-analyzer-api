using API.Analyzer.Domain.DTOs;
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
       
        //Check if it works
        public async Task<ICollection<GetStockResponseDTO>>UserStocksInWallet(string walletId)
        {
            string getUrl = $"/api/Stock/GetStocksInWallet/{walletId}";

            HttpResponseMessage response = await accountsClient.GetAsync(getUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ICollection<GetStockResponseDTO>>(jsonContent);
                
                if (result.Sum(stock => stock.Quantity) == 0)
                {
                    Console.WriteLine("There are no shares in this account.");
                }
                return result;
                
            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }
        }

        public async Task<decimal?> GetShareValue(string walletId)
        {
            try
            {
                string getUrl = $"/api/Transaction/GetTransactionsByWallet/{walletId}";
                HttpResponseMessage response = await accountsClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var transactions = JsonConvert.DeserializeObject<List<GetTransactionResponseDTO>>(jsonContent);

                    decimal totalAmount = transactions.Sum(transaction => transaction.TotalAmount);
                    int totalQuantity = transactions.Sum(transaction => transaction.Quantity);

                    if (totalQuantity != 0)
                    {
                        decimal shareValue = totalAmount / totalQuantity;
                        return shareValue;
                    }
                    else
                    {
                        Console.WriteLine("Total quantity is zero. Unable to calculate share value.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                    throw new Exception($"HTTP Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving transaction information: {ex.Message}");
                throw;
            }
        }

        public async Task<decimal?> CalculateInvestmentPercentageGain(string symbol, decimal shareValue)
        {
            try
            {
                string getUrl = $"/api/stocks/{symbol}";
                HttpResponseMessage response = await stocksClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var stockData = JsonConvert.DeserializeObject<StockData>(jsonContent);

                    decimal closePrice = (decimal)stockData.Close;
                    decimal purchasePrice = shareValue;

                    if (purchasePrice != 0) 
                    {
                        decimal investmentPercentageGain = ((closePrice - purchasePrice) / purchasePrice) * 100;
                        return investmentPercentageGain;
                    }
                    else
                    {
                        Console.WriteLine("Purchase price is zero. Unable to calculate investment percentage gain.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                    throw new Exception($"HTTP Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving stock data: {ex.Message}");
                throw;
            }
        }
    }
}
