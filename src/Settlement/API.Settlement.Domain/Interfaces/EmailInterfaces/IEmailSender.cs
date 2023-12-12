using API.Settlement.Domain.Entities.Emails;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.EmailInterfaces
{
	public interface IEmailSender
	{
		public Task SendEmail(MimeMessage email);

	}
}
