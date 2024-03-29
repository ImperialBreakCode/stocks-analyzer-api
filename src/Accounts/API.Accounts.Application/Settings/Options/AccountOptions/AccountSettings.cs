﻿namespace API.Accounts.Application.Settings.Options.AccountOptions
{
    public class AccountSettings
    {
        public string EmailConfirmationLink { get; set; }
        public ICollection<string> AllowedHosts { get; set; }
        public ExternalMicroservicesHosts ExternalMicroservicesHosts { get; set; }
        public AuthValues Auth { get; set; }
        public EmailConfiguration EmailConfig { get; set; }
    }
}
