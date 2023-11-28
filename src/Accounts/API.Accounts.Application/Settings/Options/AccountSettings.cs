namespace API.Accounts.Application.Settings.Options
{
    public class AccountSettings
    {
        public ICollection<string> AllowedHosts { get; set; }
        public ExternalMicroservicesHosts ExternalMicroservicesHosts { get; set; }
        public AuthValues Auth { get; set; }
        public EmailConfiguration EmailConfig { get; set; }
    }
}
