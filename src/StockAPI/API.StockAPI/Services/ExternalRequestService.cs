using API.StockAPI.Domain.InterFaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace API.StockAPI.Services
{
    public class ExternalRequestService : IExternalRequestService
    {
        private readonly HttpClient _client;
        private readonly string apiKey;

        public ExternalRequestService()
        {
            apiKey = "IMMIS8YCRHPXCOJP";
            _client = new();

        }
        public string QueryStringGenerator(string symbol, string function)
        {
            return $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&apikey={apiKey}";
        }

        public async Task<string> JsonDataGenerator(string query)
        {
            HttpResponseMessage response = await _client.GetAsync(query);

            string json = await response.Content.ReadAsStringAsync();

            return json;
        }
    }
}
