using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(Email emailDTO)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["SmtpSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(_configuration["SmtpSettings:SenderEmail"]/*emailDTO.To*/));
            email.Subject = emailDTO.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailDTO.Body };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_configuration["SmtpSettings:Server"], _configuration.GetValue<int>("SmtpSettings:Port"), SecureSocketOptions.StartTls);
				await smtp.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
				await smtp.SendAsync(email);
				await smtp.DisconnectAsync(true);
            }
        }
    }
}
