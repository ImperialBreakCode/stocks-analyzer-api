using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace API.Settlement.Application.Services.EmailServices
{
	public class EmailBuilder : IEmailBuilder
	{
		private readonly SmtpSettings _smtpSettings;

		public EmailBuilder(IOptions<SmtpSettings> smtpSettings)
		{
			_smtpSettings = smtpSettings.Value;
		}

		public MimeMessage BuildBaseEmail(BaseEmail emailDTO)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_smtpSettings.SenderEmail));
			email.To.Add(MailboxAddress.Parse(emailDTO.Receiver));
			email.Subject = emailDTO.Subject;
			email.Body = new TextPart(TextFormat.Html) { Text = emailDTO.Body };
			return email;
		}

		public MimeMessage BuildEmailIncludingAttachment(EmailWithAttachment emailDTO)
		{
			var email = BuildBaseEmail(emailDTO);

			var body = email.Body;
			var attachment = new MimePart(emailDTO.AttachmentMimeType)
			{
				Content = new MimeContent(new MemoryStream(emailDTO.Attachment)),
				ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
				ContentTransferEncoding = ContentEncoding.Base64,
				FileName = emailDTO.AttachmentFileName
			};

			var multipart = new Multipart("mixed");
			multipart.Add(body);
			multipart.Add(attachment);

			email.Body = multipart;

			return email;
		}

	}
}