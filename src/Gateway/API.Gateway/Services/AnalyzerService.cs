using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;

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

		public async Task<IActionResult> PortfolioSummary()
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> CurrentProfitability()
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> PercentageChange()
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> PortfolioRisk()
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> DailyProfitabilityChanges()
		{
			throw new NotImplementedException();
		}

	}
}
