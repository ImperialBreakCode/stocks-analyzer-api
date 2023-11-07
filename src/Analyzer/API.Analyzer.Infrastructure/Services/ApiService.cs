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

namespace API.Analyzer.Infrastructure.Services
{
    public class ApiService : IApiService {

  
        private readonly HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7291");
        }

        public async Task<Wallet> UserProfilInfo(string userId)
        {
            string getUrl = $"/api/accounts/{userId}";

            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Wallet>(jsonContent);
                return result;

            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }
            
        }
        public async Task<decimal?> ProfitablenessAccountCheck(string userId, decimal balance)
        {
            var userProfile = await UserProfilInfo(userId);
            if (userProfile != null)
            {
                return userProfile.Balance;
            }
            return null;
        }

        public bool GetAction(string userId)
        {
            return true;
        }

    }
}

