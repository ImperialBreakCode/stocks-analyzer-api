using System.ComponentModel.DataAnnotations;

namespace API.Accounts.Application.DTOs.Request
{
    public class LoginUserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
