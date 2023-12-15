using API.Gateway.Domain.Interfaces.Helpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Gateway.Helpers
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
            return GetClaimValueFromToken("user");
        }

        public string GetEmailFromToken()
        {
            return GetClaimValueFromToken("email");
        }

        private string GetClaimValueFromToken(string claimType)
        {
            try
            {
                var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(jwtToken))
                {
                    return null;
                }

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

                string claimValue = jsonToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
                return claimValue;
            }
            catch (Exception ex)
            {
                Log.Error($"Error parsing token: {ex.Message}");
                return string.Empty;
            }
		}
    }
}
