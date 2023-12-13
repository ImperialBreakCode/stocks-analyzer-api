using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Entities.Factories;
using API.Gateway.Domain.Entities.SQLiteEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Responses;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace API.Gateway.Services
{
    public class AccountService : IAccountService
	{
		private readonly IHttpClient _httpClient;
		private readonly MicroserviceHostsConfiguration _microserviceHosts;
		private readonly IJwtTokenParser _jwtTokenParser;
		private readonly IEmailService _emailService;
		private readonly ResponseDTOFactory _responseDTOFactory;
		private readonly ICacheHelper _cacheHelper;

		public AccountService(
			IHttpClient httpClient,
			IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts,
			IJwtTokenParser jwtTokenParser,
			IEmailService emailService,
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
			if (!_emailService.Exists(regUserDTO.Email))
			{
				return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);
			}
			else
			{
				return new ObjectResult(_responseDTOFactory.Create(ResponseMessages.BlacklistedEmail)) { StatusCode = 403 };
			}
		}

		public async Task<IActionResult> Login(LoginUserDTO userDTO)
		{
			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);
		}

		public async Task<IActionResult> UserInformation(string username)
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
				var newUser = okObjectResult.Value as User;
				var cacheOptions = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
					SlidingExpiration = TimeSpan.FromMinutes(10)
				};
				_cacheHelper.Set(cacheKey, newUser, cacheOptions);
			}

			return response;
		}

		public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			var res = await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UpdateUser/{username}", dto);

			if (res is OkObjectResult && dto.Email != null)
			{
				string email = _jwtTokenParser.GetEmailFromToken();
				await _emailService.Delete(email);

				Email mail = new() { Mail = dto.Email};
				await _emailService.Create(mail);
			}

			return res;
		}

		public async Task<IActionResult> DeleteUser()
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
	}
}
