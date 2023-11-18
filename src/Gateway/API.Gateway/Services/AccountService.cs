using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
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
		public AccountService(
			IHttpClient httpClient,
			IOptionsMonitor<MicroserviceHostsConfiguration> microserviceHosts,
			IJwtTokenParser jwtTokenParser,
			IMemoryCache memoryCache)
		{
			_httpClient = httpClient;
			_microserviceHosts = microserviceHosts.CurrentValue;
			_jwtTokenParser = jwtTokenParser;
			_memoryCache = memoryCache;
		}

		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{
			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Register", regUserDTO);
		}

		public async Task<IActionResult> Login(LoginUserDTO userDTO)
		{
			return await _httpClient.Post($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/Login", userDTO);
		}

		public async Task<IActionResult> UserInformation(string username)
		{
			if (_memoryCache.TryGetValue(username, out UserDTO dto))
			{
				return new OkObjectResult(dto);
			}

			IActionResult res = await _httpClient.Get($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UserInformation/{username}");

			UserDTO userDto = null;

			if (res is OkObjectResult okObjectResult)
			{
				userDto = okObjectResult.Value as UserDTO;
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

			_memoryCache.Set(username, userDto, cacheEntryOptions);

			return res;
		}

		public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Put($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/UpdateUser/{username}", dto);
		}

		public async Task<IActionResult> DeleteUser()
		{
			string username = _jwtTokenParser.GetUsernameFromToken();

			return await _httpClient.Delete($"{_microserviceHosts.MicroserviceHosts["Accounts"]}/User/DeleteUser/{username}");
		}

	}
}
