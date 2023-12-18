using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using API.StockAPI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;

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
                "current" => $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&interval=60min&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "daily" => $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "weekly" => $"https://www.alphavantage.co/query?function=TIME_SERIES_WEEKLY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                "monthly" => $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={symbol}&datatype=csv&apikey={apiKey}",
                _ => ""
            };
        }

        public async Task<HttpResponseMessage?> ExecuteQuery(string symbol, string query, string type)
        {
            HttpResponseMessage response = await _client.GetAsync(query);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response;
        }

        public async Task<string?> GetDataFromQuery(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public bool CheckIfDataIsValid(HttpResponseMessage? response, string? data)
        {
            if (string.IsNullOrEmpty(data)
            || data.Contains("Thank")
            || response is null
            || response.StatusCode >= HttpStatusCode.InternalServerError)
            {
                return false;
            }

            return true;
        }
    }
}
