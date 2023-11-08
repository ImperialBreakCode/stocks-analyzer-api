﻿namespace API.Accounts.Application.Settings
{
    public class AccountSettings
    {
        public ICollection<string> AllowedHosts { get; set; }
        public ExternalMicroservicesHosts ExternalMicroservicesHosts { get; set; }
        public string SecretKey { get; set; }
    }
}