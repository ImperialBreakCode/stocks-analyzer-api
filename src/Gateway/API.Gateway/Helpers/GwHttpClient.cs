using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Gateway.Helpers
{
	public class GwHttpClient : IHttpClient
	{
		private readonly HttpClient _httpClient;
		public GwHttpClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("ApiSender", "API.Gateway");
			_httpClient.Timeout = TimeSpan.FromMinutes(2);
		}

		public async Task<IActionResult> Post(string url, object obj)
		{
			var response = await _httpClient.PostAsJsonAsync(url, obj);

			return await CreateObjectResult(response);
		}

		public async Task<IActionResult> Get(string url)
		{
			var response = await _httpClient.GetAsync(url);

			return await CreateObjectResult(response);
		}

		public async Task<IActionResult> Put(string url, object obj)
		{
			var response = await _httpClient.PutAsJsonAsync(url, obj);

			return await CreateObjectResult(response);
		}

		public async Task<IActionResult> Delete(string url)
		{
			var response = await _httpClient.DeleteAsync(url);

			return await CreateObjectResult(response);
		}

		private async Task<IActionResult> CreateObjectResult(HttpResponseMessage response)
		{
			var content = await response.Content.ReadAsStringAsync();

			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.Created:
				case HttpStatusCode.Accepted:
					return new OkObjectResult(content) { StatusCode = (int)response.StatusCode };

				case HttpStatusCode.BadRequest:
					return new BadRequestObjectResult(content) { StatusCode = (int)response.StatusCode };

				default:
					return new ObjectResult(content) { StatusCode = (int)response.StatusCode };
			}
		}
	}
}
