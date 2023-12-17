namespace API.Gateway.Domain.DTOs
{
	public class AuthValues
	{
		public string SecretKey { get; set; }
		public string Audience { get; set; }
		public string Issuer { get; set; }
	}
}
