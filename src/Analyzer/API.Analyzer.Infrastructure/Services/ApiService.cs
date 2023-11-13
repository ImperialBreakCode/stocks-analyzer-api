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

namespace API.Analyzer.Infrastructure.Services
{
    public class ApiService : IApiService {

  
        private readonly HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5001");
        }

        public async Task<GetWalletResponseDTO> UserProfilInfo(string userId)
        {
            string getUrl = $"/api/accounts/{userId}";

            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

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
        public async Task<decimal?> ProfitablenessAccountCheck(string userId, decimal balance)
        {
            var userProfile = await UserProfilInfo(userId);
            if (userProfile != null)
            {
                return userProfile.Balance;
            }
            return null;
        }

        //    public decimal? GetProfitabilityForDate(string userName, DateTime date)
        //    {
        //        var result = 
        //            .Where(data => data.UserName == userName && data.Date == date)
        //            .SingleOrDefault();

        //        return result?.Profitability;
        //    }


        //    public decimal? GetProfitabilityForToday(string userName)
        //    {
        //        DateTime today = DateTime.Today;
        //        return GetProfitabilityForDate(userName, today);
        //    }

        //    public decimal? GetProfitabilityForYesterday(string userName)
        //    {
        //        DateTime yesterday = DateTime.Today.AddDays(-1);
        //        return GetProfitabilityForDate(userName, yesterday);
        //    }

        //    private readonly IAccountsDbContext accountsDbContext;

        //    public ApiService(IAccountsDbContext accountsDbContext)
        //    {
        //        this.accountsDbContext = accountsDbContext;
        //    }

       


    }
}

