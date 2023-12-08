using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.EmailServices
{
	public class EmailService : IEmailService
    {
		private readonly IEmailBuilder _emailBuilder;
		private readonly IEmailSender _emailSender;
		public EmailService(IEmailBuilder emailBuilder, IEmailSender emailSender)
		{
			_emailBuilder = emailBuilder;
			_emailSender = emailSender;
		}

		public async Task SendEmailWithoutAttachment(BaseEmail emailDTO)
        {
			var email = _emailBuilder.BuildBaseEmail(emailDTO);
			await _emailSender.SendEmail(email);
        }

		public async Task SendEmailWithAttachment(EmailWithAttachment emailDTO)
		{
			var email = _emailBuilder.BuildEmailIncludingAttachment(emailDTO);
			await _emailSender.SendEmail(email);
		}

	}
}
