using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

namespace API.StockAPI.Services
{
    public class ExternalRequestService : IExternalRequestService
    {
        //use your own damn key
        private readonly string apiKey;
        private readonly HttpClient _client;

        public ExternalRequestService(HttpClient client)
        {
            apiKey = "RYTD2DP3YZJU5IR1";
            _client = client;

        }
        public string QueryStringGenerator(string? symbol, string type)
        {
            return type switch
            {
                "current" => $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&interval=60min=&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "daily" => $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "weekly" => $"https://www.alphavantage.co/query?function=TIME_SERIES_WEEKLY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "monthly" => $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                _ => ""
            };
        }

        public async Task<string?> GetDataFromQuery(string query)
        {
            HttpResponseMessage response = await _client.GetAsync(query);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string data = await response.Content.ReadAsStringAsync();

            return data;
        }
    }
}
