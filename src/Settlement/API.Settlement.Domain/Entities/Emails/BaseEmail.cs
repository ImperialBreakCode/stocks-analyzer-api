namespace API.Settlement.Domain.Entities.Emails
{
    public class BaseEmail
    {
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
