using API.Accounts.Application.Settings;
using API.Accounts.Application.Settings.Options;
using API.Accounts.Application.Settings.UpdateHandlers;
using Microsoft.Extensions.Options;

namespace API.Accounts.Implementations
{
    public class AccountSettingsAdapter : IAccountsSettingsManager
    {
        private readonly IOptionsMonitor<AccountSettings> _settings;
        private IDisposable? _onChangeListenerDisposable;
        private readonly IAuthTokenGatewayNotifyer _authTokenGatewayNotifyer;

        public AccountSettingsAdapter(IOptionsMonitor<AccountSettings> settings, IAuthTokenGatewayNotifyer authTokenGatewayNotifyer)
        {
            _settings = settings;
            _authTokenGatewayNotifyer = authTokenGatewayNotifyer;
        }

        public ICollection<string> AllowedHosts
            => _settings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts ExternalHosts
            => _settings.CurrentValue.ExternalMicroservicesHosts;

        public string SecretKey
            => _settings.CurrentValue.Auth.SecretKey;

        public AuthValues AuthSettings
            => _settings.CurrentValue.Auth;

        public EmailConfiguration EmailConfiguration 
            => _settings.CurrentValue.EmailConfig;

        public void Dispose()
        {
            _onChangeListenerDisposable?.Dispose();
        }

        public void SetupOnChangeHandlers()
        {
            _authTokenGatewayNotifyer.NotifyGateway(AuthSettings, ExternalHosts.GatewaySocket);

            _onChangeListenerDisposable = _settings.OnChange(accountSettings =>
            {
                _authTokenGatewayNotifyer.NotifyGateway(accountSettings.Auth, ExternalHosts.GatewaySocket);
            });
        }
    }
}
