using API.Accounts.Application.Services.HttpService;

namespace API.Accounts.Implementations
{
    public class HttpServiceDecorator : IHttpService
    {
        private readonly IHttpService _httpService;

        public HttpServiceDecorator(IHttpClientFactory httpClientFactory)
        {
            HttpClient httpClient = httpClientFactory.CreateClient();
            _httpService = new HttpService(httpClient);
            
            httpClient.DefaultRequestHeaders.Add("ApiSender", "Api.Accounts");
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            return await _httpService.GetAsync<T>(url);
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            return await _httpService.PostAsync<T>(url, data);
        }

        public void Dispose()
        {
            _httpService.Dispose();
        }
    }
}
