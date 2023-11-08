using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Services
{
	public class StocksService : IStocksService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public StocksService(IHttpClient httpClient, MicroserviceHostsConfiguration microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts;
		}

		public async Task<IActionResult> GetCurrentData(string companyName)
		{
			var res = await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/Current/{companyName}");
			return res;
		}
		public async Task<IActionResult> GetDailyData(string companyName)
		{
			var res = await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/Daily/{companyName}");
			return res;
		}
		public async Task<IActionResult> GetWeeklyData(string companyName)
		{
			var res = await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/Weekly/{companyName}");
			return res;
		}
		public async Task<IActionResult> GetMonthlyData(string companyName)
		{
			var res = await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/Monthly/{companyName}");
			return res;
		}
	}
}
