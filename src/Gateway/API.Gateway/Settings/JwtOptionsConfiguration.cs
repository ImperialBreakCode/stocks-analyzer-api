using API.Gateway.Domain.DTOs;

namespace API.Gateway.Settings
{
	public class JwtOptionsConfiguration
	{
		public string SigningKey { get; set; }
		public string Audience { get; set; }
		public string Issuer { get; set; }
	}
}
