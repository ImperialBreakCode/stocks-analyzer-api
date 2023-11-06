using API.Accounts.Application.Settings;
using Microsoft.Extensions.Options;

namespace API.Accounts.Implementations
{
    public class AccountSettingsAdapter : IAccountsSettingsManager
    {
        private readonly IOptionsMonitor<AccountSettings> _settings;

        public AccountSettingsAdapter(IOptionsMonitor<AccountSettings> settings)
        {
            _settings = settings;
        }

        public ICollection<string> GetAllowedHosts
            => _settings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts GetExternalHosts
            => _settings.CurrentValue.ExternalMicroservicesHosts;

        public string GetSecretKey
            => _settings.CurrentValue.SecretKey;
    }
}
