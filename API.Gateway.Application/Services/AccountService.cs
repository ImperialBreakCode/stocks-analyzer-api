using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Entities.Factories;
using API.Gateway.Domain.Entities.SQLiteEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Helpers;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Domain.Responses;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace API.Gateway.Services
{
	public class AccountService : IAccountService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		private readonly IJwtTokenParser _jwtTokenParser;
		private readonly IEmailRepository _emailService;
		private readonly ResponseDTOFactory _responseDTOFactory;
		private readonly ICacheHelper _cacheHelper;

		public AccountService(
			IHttpClient httpClient,
			IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts,
			IJwtTokenParser jwtTokenParser,
			IEmailRepository emailService,
			ResponseDTOFactory responseDTOFactory,
			ICacheHelper cacheHelper)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
			_jwtTokenParser = jwtTokenParser;
			_emailService = emailService;
			_responseDTOFactory = responseDTOFactory;
			_cacheHelper = cacheHelper;

		}

		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{
			try
			{
				if (!_emailService.Exists(regUserDTO.Email))
				{
					return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);
				}
				else
				{
					return new ObjectResult(_responseDTOFactory.Create(ResponseMessages.BlacklistedEmail)) { StatusCode = 403 };
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AccountService, Register method: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

		public async Task<IActionResult> Login(LoginUserDTO userDTO)
		{
			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);
		}

		public async Task<IActionResult> UserInformation(string username)
		{
			try
			{
				var cacheKey = $"UserData_{username}";

				var cachedUser = _cacheHelper.Get<User>(cacheKey);

				if (cachedUser != null)
				{
					return new OkObjectResult(cachedUser);
				}

				var response = await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UserInformation/{username}");

				if (response is OkObjectResult okObjectResult)
				{
					var newUser = JsonConvert.DeserializeObject<User>((string)okObjectResult.Value);

					_cacheHelper.Set(cacheKey, newUser, 15, 10);
				}

				return response;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AccountService, UserInformation method: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

		public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
		{
			try
			{
				string username = _jwtTokenParser.GetUsernameFromToken();

				var res = await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UpdateUser/{username}", dto);

				if (res is OkObjectResult && dto.Email != null)
				{
					string email = _jwtTokenParser.GetEmailFromToken();
					await _emailService.Delete(email);

					Email mail = new() { Mail = dto.Email };
					await _emailService.Create(mail);
				}

				return res;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AccountService, UpdateUser method: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

		public async Task<IActionResult> DeleteUser()
		{
			try
			{
				string username = _jwtTokenParser.GetUsernameFromToken();

				var res = await _httpClient.Delete($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/DeleteUser/{username}");

				if (res is OkObjectResult)
				{
					string email = _jwtTokenParser.GetEmailFromToken();
					await _emailService.Delete(email);
				}

				return res;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AccountService, DeleteUser method: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

		public async Task<IActionResult> ConfirmUser(string userId)
		{
			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/ConfirmUser/{userId}");
		}

		public async Task<IActionResult> GetTransactions()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/Transaction/GetTransactionsByUsername/{username}");
		}
	}
}
