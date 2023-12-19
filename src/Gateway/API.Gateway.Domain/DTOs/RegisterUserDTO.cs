using System.ComponentModel.DataAnnotations;

namespace API.Gateway.Domain.DTOs
{
	public class RegisterUserDTO
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
