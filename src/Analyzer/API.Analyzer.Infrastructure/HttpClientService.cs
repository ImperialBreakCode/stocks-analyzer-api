using System;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using API.Analyzer.Domain.DTOs;
using Newtonsoft.Json.Linq;
using API.Analyzer.Domain.Interface;

namespace API.Analyzer
{
    public class HttpClientService : IService
    {
        private readonly HttpClient httpClient;

        public HttpClientService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5089");
        }


        public async Task<User> UserProfilInfo(int id)
        {
            string getUrl = $"/api/accounts/{id}";

            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<User>(jsonContent);
                return result;

            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
            }
            return null;
        }
        public bool ProfitablenessAccountCheck(int id, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}




    //Calculation formulas
    //EPS = (Net Income) / (Number of Common Shares Outstanding) // MFS // EPS = (20.08 billion) / (7.43 billion) EPS = 2.7021
    //Нетни доходи//         //Акции в обръщение//


