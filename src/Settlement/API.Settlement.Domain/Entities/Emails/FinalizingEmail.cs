namespace API.Settlement.Domain.Entities.Emails
{
	public class FinalizingEmail
	{
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public byte[] Attachment { get; set; }
		public string AttachmentFileName { get; set; }
		public string AttachmentMimeType { get; set; }
	}
}
