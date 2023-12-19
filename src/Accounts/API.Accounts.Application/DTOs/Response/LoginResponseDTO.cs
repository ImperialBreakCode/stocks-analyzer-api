namespace API.Accounts.Application.DTOs.Response
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO()
        {
            Token = String.Empty;
        }

        public string Token { get; set; }
        public string Message { get; set; }
    }
}
