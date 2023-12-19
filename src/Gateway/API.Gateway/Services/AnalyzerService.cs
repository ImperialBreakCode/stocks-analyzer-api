using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
    public class AnalyzerService : IAnalyzerService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;

		public AnalyzerService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
		}

		public async Task<IActionResult> PortfolioSummary(string walletId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/User/PortfolioSummary/{walletId}");
		}

		public async Task<IActionResult> CurrentBalanceInWallet(string walletId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/User/CurrentBalanceInWallet/{walletId}");
		}

		public async Task<IActionResult> GetUserStocksInWallet(string walletId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/Stock/GetUserStocksInWallet/{walletId}");
		}

		public async Task<IActionResult> CurrentProfitability(string username, string symbol, string type)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/Stock/CurrentProfitability/{username}/{symbol}/{type}");
		}

		public async Task<IActionResult> PercentageChange(string username, string symbol, string type)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/Stock/PercentageChange/{username}/{symbol}/{type}");
		}

		public async Task<IActionResult> CalculateAverageProfitability(string username, string symbol, string type)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Analyzer"]}/Stock/CalculateAverageProfitability/{username}/{symbol}/{type}");
		}
	}
}
