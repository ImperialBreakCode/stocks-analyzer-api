using API.Settlement.Domain.Entities.Emails;

namespace API.Settlement.Domain.Interfaces.EmailInterfaces
{
	public interface IEmailService
	{
		Task SendEmailWithoutAttachment(BaseEmail emailDTO);
		Task SendEmailWithAttachment(EmailWithAttachment emailDTO);
	}
}
