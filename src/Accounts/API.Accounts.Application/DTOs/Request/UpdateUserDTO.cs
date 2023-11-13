namespace API.Accounts.Application.DTOs.Request
{
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
