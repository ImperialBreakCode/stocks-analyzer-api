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

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return new OkObjectResult(content);
			}
			else
			{
				return new StatusCodeResult((int)response.StatusCode);
			}
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
		public async Task<string> GetStringAsync(string url)
		{
			var response = await _httpClient.GetAsync(url);
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
		public async Task<IActionResult> GetActionResult(string url)
		{
			var response = await _httpClient.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return new OkObjectResult(content);
			}
			else
			{
				return new StatusCodeResult((int)response.StatusCode);
			}
		}
	}
}
