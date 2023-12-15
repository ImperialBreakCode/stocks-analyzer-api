using API.Accounts.Application.Settings;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace API.Accounts.Application.Services.UserService.EmailService
{
    internal class EmailConfirmation : IEmailConfirmation
    {
        private readonly IAccountsSettingsManager _settingsManager;

        public EmailConfirmation(IAccountsSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public bool SendEmail(string email, string userId)
        {
            try
            {
                var mimeMessage = CreateMimeMessage(email, userId);
                CreateSmtpAndSendMail(mimeMessage);
                return true;
            }
            catch (SmtpCommandException)
            {
                return false;
            }
        }

        private void CreateSmtpAndSendMail(MimeMessage mimeMessage)
        {
            using var smtpClient = new SmtpClient();
            smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true; // only for testing
            smtpClient.Connect(
                _settingsManager.EmailConfiguration.SmtpServer,
                _settingsManager.EmailConfiguration.SmtpPort,
                MailKit.Security.SecureSocketOptions.Auto);
            smtpClient.Authenticate(_settingsManager.EmailConfiguration.Username, _settingsManager.EmailConfiguration.Password);
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
        }

        private MimeMessage CreateMimeMessage(string email, string userId)
        {
            var mime = new MimeMessage();
            mime.From.Add(MailboxAddress.Parse(_settingsManager.EmailConfiguration.Sender));
            mime.To.Add(MailboxAddress.Parse(email));
            mime.Subject = "Confirm Your Account";
            mime.Body = new TextPart(TextFormat.Html) { Text = LoadEmailTemlate(userId) };

            return mime;
        }

        private string LoadEmailTemlate(string userId)
        {
            return string.Format(EmailTemplate.HtmlTemplate, $"https://localhost:5032/api/User/ConfirmUser/{userId}", EmailTemplate.Styles);
        }
    }
}
