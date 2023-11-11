using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
	public class AccountService : IAccountService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public AccountService(IHttpClient httpClient, IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
		}

		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{
			return await _httpClient.PostAsJsonAsync($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);
		}

		public async Task<IActionResult> Login(UserDTO userDTO)
		{
			return await _httpClient.PostAsJsonAsync($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);
		}
		public async Task<IActionResult> UserInformation(string username)
		{
			return await _httpClient.GetActionResult($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UserInformation/{username}");
		}
	}
}
