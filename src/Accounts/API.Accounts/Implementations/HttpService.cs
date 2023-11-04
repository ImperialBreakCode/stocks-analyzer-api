using API.Accounts.Application.Services.HttpService;
using Newtonsoft.Json;

namespace API.Accounts.Implementations
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonData = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task PostAsync(string url, object data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
