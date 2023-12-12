namespace API.Settlement.Domain.Entities.Emails
{
	public class EmailWithAttachment : BaseEmail
	{
		public byte[] Attachment { get; set; }
		public string AttachmentFileName { get; set; }
		public string AttachmentMimeType { get; set; }
	}
}
