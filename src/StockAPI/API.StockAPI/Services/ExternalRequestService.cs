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
        public async Task<string?> GetData(string symbol, string type)
        {
            var query = QueryStringGenerator(symbol, type);
            if(query == null)
            {
                return null;
            }

            var data = await GetDataFromQuery(query);
            if(data == null)
            {
                return null;
            }

            return data;
        }
        public string QueryStringGenerator(string symbol, string type)
        {
            string function = "";
            switch(type)
            {
                case "current":
                    function = "TIME_SERIES_INTRADAY&interval=60min";
                    break;
                case "daily":
                    function = "TIME_SERIES_DAILY";
                    break;
                case "weekly":
                    function = "TIME_SERIES_WEEKLY";
                    break;
                case "monthly":
                    function = "TIME_SERIES_MONTHLY";
                    break;
                default:
                    break;
            }

            return $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&datatype=csv&apikey={apiKey}";
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
