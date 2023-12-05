using API.Settlement.Domain.Entities.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
    public interface IEmailService
	{
		Task SendEmailWithoutAttachment(NotifyingEmail emailDTO);
		Task SendEmailWithAttachment(FinalizingEmail emailDTO);
	}
}
