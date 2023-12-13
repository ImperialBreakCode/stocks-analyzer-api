using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
    public class StockInfoService : IStockInfoService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public StockInfoService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
		}

		public async Task<IActionResult> GetCurrentData(string companyName)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/current/{companyName}");
		}

		public async Task<IActionResult> GetDailyData(string companyName)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/daily/{companyName}");
		}

		public async Task<IActionResult> GetWeeklyData(string companyName)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/weekly/{companyName}");
		}

		public async Task<IActionResult> GetMonthlyData(string companyName)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["StockAPI"]}/Stock/monthly/{companyName}");
		}

	}
}
