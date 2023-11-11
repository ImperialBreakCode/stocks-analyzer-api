using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
	public class StockService : IStockService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public StockService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
		}

		public async Task<IActionResult> GetStock(string stockId)
		{
			return await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/GetStock/{stockId}");
		}

		public async Task<IActionResult> GetStocksInWallet(string walletId)
		{
			return await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/GetStocksInWallet/{walletId}");
		}

		public async Task<IActionResult> AddStockForPurchase(StockDTO dto)
		{
			return await _httpClient.PutActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase", dto);
		}
		public async Task<IActionResult> AddStockForSale(StockDTO dto)
		{
			return await _httpClient.PutActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase", dto);
		}

		public async Task<IActionResult> ConfirmPurchase(string walletId)
		{
			return await _httpClient.PostActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase", walletId);
		}
		public async Task<IActionResult> ConfirmSale(string walletId)
		{
			return await _httpClient.PostActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Stock/AddStockForPurchase", walletId);
		}


	}
}
