using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Extensions;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
	public class WalletService : IWalletService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		private readonly IJwtTokenParser _jwtTokenParser;
		public WalletService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts, IJwtTokenParser jwtTokenParser)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
			_jwtTokenParser = jwtTokenParser;
		}

		public async Task<IActionResult> Deposit(DepositWalletDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/Deposit/{username}", dto);
		}

		public async Task<IActionResult> CreateWallet()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/CreateWallet/{username}", null);
		}

		public async Task<IActionResult> DeleteWallet()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Delete($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/DeleteWallet/{username}");
		}
		//public async Task<IActionResult> GetWallet(string walletId)
		//{
		//	return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Wallet/GetWallet/{walletId}");
		//}

	}
}
