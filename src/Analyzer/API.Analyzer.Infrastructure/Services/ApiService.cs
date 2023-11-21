using System;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using API.Analyzer.Domain.DTOs;
using Newtonsoft.Json.Linq;
using API.Analyzer.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;
using API.StockAPI.Domain.Models;

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

        //Connection with AccountsAPI
        public async Task<GetWalletResponseDTO> UserProfilInfo(string userId)
        {
            string getUrl = $"/api/accounts/{userId}";

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

        public async Task<decimal?> CurrentProfitability(string userName)
        {
            var userProfile = await UserProfilInfo(userName);
            if (userProfile != null)
            {
                return userProfile.Balance;
            }
            return null;
        }

        public async Task<decimal> PercentageChange(string symbol)
        {
            try
            {
                StockData stockResponse = await GetStockInfo(symbol);

                decimal currentNetIncome = stockResponse.NetIncome;
                decimal currentCommonSharesOutstanding = stockResponse.CommonSharesOutstanding;

                // Calculate Percentage Change
                decimal percentageChange = (currentNetIncome / currentCommonSharesOutstanding) * 100;

                return percentageChange;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving current stock percentage change: {ex.Message}");
                throw;
            }
        }

    }

}

