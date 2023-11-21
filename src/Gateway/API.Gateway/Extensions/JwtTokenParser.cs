using API.Gateway.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Gateway.Extensions
{
	public class JwtTokenParser : IJwtTokenParser
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public JwtTokenParser(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetUsernameFromToken()
		{
			var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (string.IsNullOrEmpty(jwtToken))
			{
				return null;
			}

			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

			string username = jsonToken?.Claims.FirstOrDefault(c => c.Type == "user")?.Value;
			return username;
		}
	}
}
