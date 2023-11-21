using API.Gateway.Infrastructure.Models;

namespace API.Gateway.Infrastructure.Provider
{
	public interface IEmailService
	{
		Task Create(Email email);
		Task<Email> Get();
	}
}