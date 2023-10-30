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

			[HttpPost]
		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{

			IActionResult response = await _httpClient.PostAsJsonAsync("api/User/Register", regUserDTO);

			return response;
		}
	   
		[HttpPost]
		public async Task<IActionResult> Login(UserDTO userDTO)
		{

			string response = await _httpClient.PostAsJsonAsyncReturnString("api/User/Login", userDTO);
			if (response == string.Empty) 
			{
				return new BadRequestResult();
			}

			LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

			return new OkObjectResult(loginResponse);

		}

		[HttpPut]
		public async Task<IActionResult> Deposit()
		{
			return new OkResult();
		}

		[HttpPost]
		public async Task<IActionResult> CreateWallet()
		{
			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> UserInformation()
		{
			return new OkResult();
		}


	}
}
