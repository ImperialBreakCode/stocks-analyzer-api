using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Extensions;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
    public class StockService : IStockService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		private readonly IJwtTokenParser _jwtTokenParser;

		public StockService(IHttpClient httpClient,
			IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts,
			IJwtTokenParser jwtTokenParser)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
			_jwtTokenParser = jwtTokenParser;
		}

		public async Task<IActionResult> GetStock(string stockId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/GetStock/{stockId}");
		}

		public async Task<IActionResult> GetStocksInWallet(string walletId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/GetStocksInWallet/{walletId}");
		}

		public async Task<IActionResult> AddStockForPurchase(StockDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase/{username}", dto);
		}

		public async Task<IActionResult> AddStockForSale(StockDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase/{username}", dto);
		}

		public async Task<IActionResult> ConfirmPurchase()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase/{username}", null);
		}

		public async Task<IActionResult> ConfirmSale()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase/{username}", null);
		}

	}
}
