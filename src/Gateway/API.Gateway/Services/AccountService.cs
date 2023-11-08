using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using Newtonsoft.Json;
using API.Gateway.Settings;

namespace API.Gateway.Services
{
	public class AccountService : IAccountService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		public AccountService(IHttpClient httpClient, MicroserviceHostsConfiguration microserviceHosts)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts;
		}

		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{

			 var res = await _httpClient.PostAsJsonAsync($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);

			return res;

		}
	   
		public async Task<LoginResponse> Login(UserDTO userDTO)
		{

			string response = await _httpClient.PostAsJsonAsyncReturnString($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);

			LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

			return loginResponse;

		}

		public async Task<IActionResult> Deposit()
		{
			return new OkResult();
		}

		public async Task<IActionResult> CreateWallet()
		{
			return new OkResult();
		}

		public async Task<IActionResult> UserInformation()
		{
			return new OkResult();
		}


	}
}
