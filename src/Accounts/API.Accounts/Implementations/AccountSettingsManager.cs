using API.Accounts.Application.Settings;
using Microsoft.Extensions.Options;

namespace API.Accounts.Implementations
{
    public class AccountSettingsManager : IAccountsSettingsManager
    {
        private readonly IOptionsMonitor<AccountSettings> _settings;

        public AccountSettingsManager(IOptionsMonitor<AccountSettings> settings)
        {
            _settings = settings;
        }

        public ICollection<string> GetAllowedHosts()
            => _settings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts GetExternalHosts()
            => _settings.CurrentValue.ExternalMicroservicesHosts;

        public string GetSecretKey()
            => _settings.CurrentValue.SecretKey;
    }
}
