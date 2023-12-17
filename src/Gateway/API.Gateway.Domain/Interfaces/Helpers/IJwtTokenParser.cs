namespace API.Gateway.Domain.Interfaces.Helpers
{
	public interface IJwtTokenParser
    {
        string GetUsernameFromToken();
        string GetEmailFromToken();
    }
}
