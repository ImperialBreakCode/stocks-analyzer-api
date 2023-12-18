using System.ComponentModel.DataAnnotations;

namespace API.Gateway.Domain.DTOs
{
	public class LoginUserDTO
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
