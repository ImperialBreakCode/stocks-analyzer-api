
namespace API.Accounts.Application.Auth.TokenManager
{
    public interface ITokenManager
    {
        string CreateToken(string username, string email, int secondsValid);
    }
}
