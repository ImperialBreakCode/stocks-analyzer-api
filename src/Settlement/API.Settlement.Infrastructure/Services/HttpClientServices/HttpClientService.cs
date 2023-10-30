using API.Settlement.Domain.Interfaces;
using System.Text;

namespace API.Settlement.Infrastructure.Services.HttpClientServices
{
	public class HttpClientService : IHttpClientService
	{
		private readonly HttpClient _httpClientService;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClientService = httpClient;
        }
		public async Task<string> GetStringAsync(string uri)
		{
			var response = await _httpClientService.GetAsync(uri);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<string> PostAsync(string uri, string message)
		{
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var response = await _httpClientService.PostAsync(uri, content);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<string> DeleteAsync(string uri)
		{
			var response = await _httpClientService.DeleteAsync(uri);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

		
	}
}