using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Gateway.Services
{
	public class GwHttpClient : IHttpClient
	{
		private HttpClient _httpClient;
		public GwHttpClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		public async Task<string> PostAsJsonAsyncReturnString(string url, object obj)
		{
			var response = await _httpClient.PostAsJsonAsync(url, obj);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch (Exception)
			{

				return string.Empty;
			}
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<IActionResult> PostAsJsonAsync(string url, object obj)
		{
			var response = await _httpClient.PostAsJsonAsync(url, obj);

			return (IActionResult)response;
		}

		public async Task<string> PostAsync(string url, string message)
		{
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content);

			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch (Exception)
			{

				return string.Empty;
			}
			return await response.Content.ReadAsStringAsync();

		}
	}
}
