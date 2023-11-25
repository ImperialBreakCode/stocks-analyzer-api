using API.Gateway.Infrastructure.Models;

namespace API.Gateway.Infrastructure.Provider
{
	public interface IEmailService
	{
		Task Create(Email email);
		bool Exists(string email);
		Task Delete(string email);
	}
}