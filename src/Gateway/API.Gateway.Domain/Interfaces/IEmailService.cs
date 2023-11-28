using API.Gateway.Domain.DTOs;

namespace API.Gateway.Domain.Interfaces
{
	public interface IEmailService
	{
		Task Create(Email email);
		bool Exists(string email);
		Task Delete(string email);
	}
}