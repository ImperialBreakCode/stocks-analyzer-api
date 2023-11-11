using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
	public class WalletService : IWalletService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public WalletService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
		}

		public async Task<IActionResult> CreateWallet(string username)
		{
			return await _httpClient.PostActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/CreateWallet/{username}", null);
		}
		public async Task<IActionResult> GetWallet(string walletId)
		{
			return await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/GetWallet/{walletId}");
		}

	}
}
