using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using Newtonsoft.Json;


namespace API.Gateway.Services
{
	public class AccountService : IAccountService
	{
		private IHttpClient _httpClient;
		public AccountService(IHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task Register(RegisterUserDTO regUserDTO)
		{

			 await _httpClient.PostAsJsonAsync("api/User/Register", regUserDTO);

		}
	   
		public async Task<LoginResponse> Login(UserDTO userDTO)
		{

			string response = await _httpClient.PostAsJsonAsyncReturnString("api/User/Login", userDTO);

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
