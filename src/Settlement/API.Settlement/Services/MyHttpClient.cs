using API.Settlement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services
{
	public class MyHttpClient : IHttpClient
	{
		private HttpClient _httpClient;

        public MyHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
		public async Task<string> GetStringAsync(string uri)
		{
			var response = await _httpClient.GetAsync(uri);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<string> PostAsync(string uri, string message)
		{
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(uri, content);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<string> DeleteAsync(string uri)
		{
			var response = await _httpClient.DeleteAsync(uri);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

		
	}
}
