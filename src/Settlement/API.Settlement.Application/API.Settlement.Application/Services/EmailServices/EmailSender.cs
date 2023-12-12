using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Settlement.Application.Services.EmailServices
{
	public class EmailSender : IEmailSender
	{
		private readonly SmtpSettings _smtpSettings;
		public EmailSender(IOptions<SmtpSettings> smtpSettings)
		{
			_smtpSettings = smtpSettings.Value;
		}

		public async Task SendEmail(MimeMessage email)
		{
			using (var smtp = new SmtpClient())
			{
				await smtp.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
				await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
				await smtp.SendAsync(email);
				await smtp.DisconnectAsync(true);
			}
		}

	}
}
