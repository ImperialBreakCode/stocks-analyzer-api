using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Gateway.Services
{
	public class GwHttpClient : IHttpClient
	{
		private readonly HttpClient _httpClient;
		public GwHttpClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("ApiSender", "API.Gateway");
		}

		public async Task<IActionResult> Post(string url, object obj)
		{
			var response = await _httpClient.PostAsJsonAsync(url, obj);

			return new ObjectResult(await response.Content.ReadAsStringAsync())
			{
				StatusCode = (int)response.StatusCode
			};
		}

		public async Task<IActionResult> Get(string url)
		{
			var response = await _httpClient.GetAsync(url);

			return new ObjectResult(await response.Content.ReadAsStringAsync())
			{
				StatusCode = (int)response.StatusCode
			};
		}

		public async Task<IActionResult> Put(string url, object obj)
		{
			var response = await _httpClient.PutAsJsonAsync(url, obj);

			return new ObjectResult(await response.Content.ReadAsStringAsync())
			{
				StatusCode = (int)response.StatusCode
			};
		}

		public async Task<IActionResult> Delete(string url)
		{
			var response = await _httpClient.DeleteAsync(url);

			return new ObjectResult(await response.Content.ReadAsStringAsync())
			{
				StatusCode = (int)response.StatusCode
			};
		}

	}
}
