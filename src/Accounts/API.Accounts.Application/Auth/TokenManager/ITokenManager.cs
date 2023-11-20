
namespace API.Accounts.Application.Auth.TokenManager
{
    public interface ITokenManager
    {
        string CreateToken(string username, int secondsValid);
    }
}
