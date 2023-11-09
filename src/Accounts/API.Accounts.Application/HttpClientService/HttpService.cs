using Newtonsoft.Json;
using System.Text;

namespace API.Accounts.Application.HttpClientService
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonData = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
