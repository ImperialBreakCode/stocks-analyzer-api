using API.Settlement.Domain.Entities.Emails;

namespace API.Settlement.Domain.Interfaces.EmailInterfaces
{
	public interface IEmailService
	{
		Task SendEmailWithoutAttachment(NotifyingEmail emailDTO);
		Task SendEmailWithAttachment(FinalizingEmail emailDTO);
	}
}
