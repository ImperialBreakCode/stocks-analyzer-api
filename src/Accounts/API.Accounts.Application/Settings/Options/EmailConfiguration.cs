namespace API.Accounts.Application.Settings.Options
{
    public class EmailConfiguration
    {
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
