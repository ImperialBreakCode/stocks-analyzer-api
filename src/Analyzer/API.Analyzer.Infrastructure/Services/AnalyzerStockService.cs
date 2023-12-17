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
            stocksClient.BaseAddress = new Uri("https://localhost:7160");
        }

        // Formula for Calculating Percentage Gain or Loss
        // Investment percentage gain  = ((Price sold − purchase price) / purchase price ) × 100
        public async Task<ICollection<GetStockResponseDTO>> UserStocksInWallet(string walletId)
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

        public async Task<decimal?> GetShareValue(string username)
        {
            try
            {
                string getUrl = $"/api/Transaction/GetTransactionsByUsername/{username}";
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
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }
        public async Task<StockData> GetStockData(string symbol, string type)
        {
            try
            {
                string getUrl = $"/api/Stock/{type}/{symbol}";
                HttpResponseMessage response = await stocksClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<StockData>(jsonContent);
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
        public async Task<decimal?> GetCurrentProfitability(string username, string symbol, string type)
        {
            try
            {
                decimal? shareValue = await GetShareValue(username);

                if (shareValue.HasValue)
                {
                    StockData stockData = await GetStockData(symbol, type);
                    decimal closePrice = (decimal)stockData.Close;
                    decimal currentProfitability = (closePrice * 100) - (shareValue.Value * 100);

                    decimal currentProfitabilityFormattedNumber = Math.Round(currentProfitability, 2);
                    return currentProfitabilityFormattedNumber;
                }
                else
                {
                    Console.WriteLine("Unable to retrieve share value.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCurrentProfitability: {ex.Message}");
                throw;
            }
        }



        //public async Task<decimal?> CalculateInvestmentPercentageGain(string symbol, string type, decimal shareValue)
        //{
        //    try
        //    {
        //        string getUrl = $"/api/Stock/{type}/{symbol}";
        //        HttpResponseMessage response = await stocksClient.GetAsync(getUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonContent = await response.Content.ReadAsStringAsync();
        //            var stockData = JsonConvert.DeserializeObject<StockData>(jsonContent);

        //            decimal closePrice = (decimal)stockData.Close;
        //            decimal purchasePrice = shareValue;

        //            if (purchasePrice != 0)
        //            {
        //                decimal investmentPercentageGain = ((closePrice - purchasePrice) / purchasePrice) * 100;
        //                decimal percentageGainFormattedNumber = Math.Round(investmentPercentageGain, 2);
        //                return percentageGainFormattedNumber;
        //            }
        //            else
        //            {
        //                Console.WriteLine("Purchase price is zero. Unable to calculate investment percentage gain.");
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"HTTP Error: {response.StatusCode}");
        //            string errorContent = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"Error Content: {errorContent}");
        //            throw new Exception($"HTTP Error: {response.StatusCode}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error retrieving stock data: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<decimal?> CalculateAverageProfitability(string username, string symbol, string type)
        {
            try
            {
                string transactionUrl = $"/api/Transaction/GetTransactionsByUsername/{username}";
                HttpResponseMessage transactionResponse = await accountsClient.GetAsync(transactionUrl);

                if (transactionResponse.IsSuccessStatusCode)
                {
                    string transactionJsonContent = await transactionResponse.Content.ReadAsStringAsync();
                    var transactions = JsonConvert.DeserializeObject<List<GetTransactionResponseDTO>>(transactionJsonContent);

                    if (transactions != null && transactions.Any())
                    {
                        decimal totalInvestmentPercentageGain = 0;

                        foreach (var transaction in transactions)
                        {
                            decimal totalAmount = transaction.TotalAmount;
                            int totalQuantity = transaction.Quantity;

                            if (totalQuantity != 0)
                            {
                                decimal shareValue = totalAmount / totalQuantity;

                                string stockUrl = $"/api/Stock/{type}/{symbol}";
                                HttpResponseMessage stockResponse = await stocksClient.GetAsync(stockUrl);

                                if (stockResponse.IsSuccessStatusCode)
                                {
                                    string stockJsonContent = await stockResponse.Content.ReadAsStringAsync();
                                    var stockData = JsonConvert.DeserializeObject<StockData>(stockJsonContent);

                                    decimal closePrice = (decimal)stockData.Close;
                                    decimal purchasePrice = shareValue;

                                    if (purchasePrice != 0)
                                    {
                                        decimal investmentPercentageGain = ((closePrice - purchasePrice) / purchasePrice) * 100;
                                        totalInvestmentPercentageGain += investmentPercentageGain;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Unable to calculate investment percentage gain.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"HTTP Error: {stockResponse.StatusCode}");
                                    string errorContent = await stockResponse.Content.ReadAsStringAsync();
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Unable to calculate share value.");
                            }
                        }

                        decimal averageProfitability = totalInvestmentPercentageGain / transactions.Count;
                        decimal avProfitabilityFormattedNumber = Math.Round(averageProfitability, 2);
                        return avProfitabilityFormattedNumber;
                    }
                    else
                    {
                        Console.WriteLine("Unable to calculate average profitability.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {transactionResponse.StatusCode}");
                    throw new Exception($"HTTP Error: {transactionResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating average profitability: {ex.Message}");
                throw;
            }
        }

        //    public async Task <List<string?>> DailyProfitabilityChanges(string symbol,string type)
        //    {
        //        List<string?> dates = new List<string?>();

        //        string getUrl = $"/api/Stock/{type}/{symbol}";

        //        HttpResponseMessage response = await stocksClient.GetAsync(getUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonContent = await response.Content.ReadAsStringAsync();
        //            var stockDataList = JsonConvert.DeserializeObject<List<StockData>>(jsonContent);

        //            foreach (var stockData in stockDataList)
        //            {
        //                dates.Add(stockData.Date);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Error: {response.StatusCode}");
        //        }

        //        return dates;
        //    }

        public async Task<List<decimal>> PersentageChange(string username, string symbol, string type)
        {
            try
            {
                string transactionUrl = $"/api/Transaction/GetTransactionsByUsername/{username}";
                HttpResponseMessage transactionResponse = await accountsClient.GetAsync(transactionUrl);

                if (transactionResponse.IsSuccessStatusCode)
                {
                    string transactionJsonContent = await transactionResponse.Content.ReadAsStringAsync();
                    var transactions = JsonConvert.DeserializeObject<List<GetTransactionResponseDTO>>(transactionJsonContent);

                    if (transactions != null && transactions.Any())
                    {
                        List<decimal> investmentPercentageGains = new List<decimal>();

                        foreach (var transaction in transactions)
                        {
                            decimal totalAmount = transaction.TotalAmount;
                            int totalQuantity = transaction.Quantity;

                            if (totalQuantity != 0)
                            {
                                decimal shareValue = totalAmount / totalQuantity;

                                string stockUrl = $"/api/Stock/{type}/{symbol}";
                                HttpResponseMessage stockResponse = await stocksClient.GetAsync(stockUrl);

                                if (stockResponse.IsSuccessStatusCode)
                                {
                                    string stockJsonContent = await stockResponse.Content.ReadAsStringAsync();
                                    var stockData = JsonConvert.DeserializeObject<StockData>(stockJsonContent);

                                    decimal closePrice = (decimal)stockData.Close;
                                    decimal purchasePrice = shareValue;

                                    if (purchasePrice != 0)
                                    {
                                        decimal investmentPercentageGain = ((closePrice - purchasePrice) / purchasePrice) * 100;
                                        investmentPercentageGains.Add(Math.Round(investmentPercentageGain, 2));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Unable to calculate investment percentage gain for transaction because purchase price is zero.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"HTTP Error: {stockResponse.StatusCode}");
                                    string errorContent = await stockResponse.Content.ReadAsStringAsync();
                                    Console.WriteLine($"Error Content: {errorContent}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Unable to calculate investment percentage gain for transaction because total quantity is zero.");
                            }
                        }

                        return investmentPercentageGains;
                    }
                    else
                    {
                        Console.WriteLine("Unable to calculate investment percentage gain. No transactions found for the specified username.");
                        return new List<decimal>();
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {transactionResponse.StatusCode}");
                    throw new Exception($"HTTP Error: {transactionResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating: {ex.Message}");
                throw;
            }
        }

    }
}

