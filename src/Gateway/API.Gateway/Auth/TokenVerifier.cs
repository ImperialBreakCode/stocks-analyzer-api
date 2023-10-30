using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Gateway.Auth
{
	public class TokenVerifier
	{
		private readonly IConfiguration _configuration;
		private readonly string _secretKey;

		public TokenVerifier(IConfiguration configuration)
		{
			_configuration = configuration;
			_secretKey = _configuration["Jwtoptions:SigningKey"];
		}

		public bool ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Convert.FromHexString(_secretKey)),
				ValidateIssuer = true,
				ValidateAudience = true,
				ClockSkew = TimeSpan.Zero
			};

			try
			{
				SecurityToken validatedToken;
				var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
				return true;
			}
			catch (SecurityTokenException ex)
			{
				Console.WriteLine($"Token validation failed: {ex.Message}");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
				return false;
			}
		}
	}
}
