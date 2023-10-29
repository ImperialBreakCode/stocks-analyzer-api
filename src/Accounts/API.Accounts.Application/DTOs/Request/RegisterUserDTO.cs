using System.ComponentModel.DataAnnotations;

namespace API.Accounts.Application.DTOs.Request
{
    public class RegisterUserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
