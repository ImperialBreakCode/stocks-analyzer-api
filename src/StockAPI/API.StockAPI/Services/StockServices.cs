using API.StockAPI.Models;
using Newtonsoft.Json;

namespace API.StockAPI.Services
{
    public class StockServices
    {
        private readonly string apiKey;

        public StockServices()
        {
            apiKey = "";
        }
        public async Task<TimeSeriesData> GetCurrentStock(string symbol, string function)
        {
            string response = await JsonDataGenerator(symbol, function);
            StockData? data = JsonConvert.DeserializeObject<StockData>(response);

            var timeSeriesData = data.TimeSeries;
            timeSeriesData.First().Value.Symbol = symbol;
            timeSeriesData.First().Value.Date = timeSeriesData.First().Key;

            var result = timeSeriesData.First().Value;
            return result;
        }

        public async Task<string> JsonDataGenerator(string symbol, string function)
        {
            HttpClient _client = new HttpClient();

            string _url = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&interval=60min&apikey={apiKey}";

            HttpResponseMessage response = await _client.GetAsync(_url);

            string json = await response.Content.ReadAsStringAsync();

            return json;
        }
    }
}
