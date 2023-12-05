using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Entities.SQLiteEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Infrastructure.Provider;
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
		private readonly IMemoryCache _memoryCache;
		private readonly IEmailService _emailService;
		public AccountService(
			IHttpClient httpClient,
			IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts,
			IJwtTokenParser jwtTokenParser,
			IMemoryCache memoryCache,
			IEmailService emailService)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
			_jwtTokenParser = jwtTokenParser;
			_memoryCache = memoryCache;
			_emailService = emailService;
		}

		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{
			if (!_emailService.Exists(regUserDTO.Email))
			{
				return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);
			}
			else
			{
				ResponseDTO responseDTO = new ResponseDTO()
				{
					Message = $"The email '{regUserDTO.Email}' has already been used or is blacklisted!"
				};
				return new ObjectResult(responseDTO)
				{
					StatusCode = 403
				};
			}
		}

		public async Task<IActionResult> Login(LoginUserDTO userDTO)
		{
			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);
		}

		public async Task<IActionResult> UserInformation(string username)
		{
			if (_memoryCache.TryGetValue(username, out User dto))
			{
				return new OkObjectResult(dto);
			}

			IActionResult res = await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UserInformation/{username}");

			User user = null;

			if (res is OkObjectResult okObjectResult)
			{
				user = okObjectResult.Value as User;
			}
			else
			{
				return res;
			}

			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
				SlidingExpiration = TimeSpan.FromMinutes(10)
			};

			_memoryCache.Set(username, user, cacheEntryOptions);

			return res;
		}

		public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			ObjectResult res = (ObjectResult)await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UpdateUser/{username}", dto);

			if (res is OkObjectResult && dto.Email != null)
			{
				string email = _jwtTokenParser.GetEmailFromToken();
				await _emailService.Delete(email);
				Email mail = new Email
				{
					Mail = dto.Email
				};
				await _emailService.Create(mail);
			}

			return res;
		}

		public async Task<IActionResult> DeleteUser()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			ObjectResult res = (ObjectResult)await _httpClient.Delete($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/DeleteUser/{username}");

			if (res is OkObjectResult) 
			{
				string email = _jwtTokenParser.GetEmailFromToken();
				await _emailService.Delete(email);
			}

			return res;

		}

	}
}
