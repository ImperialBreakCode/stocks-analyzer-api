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
        private readonly IAuthTokenGatewayNotifyer _secretKeyGatewayNotifyer;

        public AccountSettingsAdapter(IOptionsMonitor<AccountSettings> settings, IAuthTokenGatewayNotifyer secretKeyGatewayNotifyer)
        {
            _settings = settings;
            _secretKeyGatewayNotifyer = secretKeyGatewayNotifyer;
        }

        public ICollection<string> GetAllowedHosts
            => _settings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts GetExternalHosts
            => _settings.CurrentValue.ExternalMicroservicesHosts;

        public string GetSecretKey
            => _settings.CurrentValue.Auth.SecretKey;

        public AuthValues GetAuthSettings
            => _settings.CurrentValue.Auth;


        public void Dispose()
        {
            _onChangeListenerDisposable?.Dispose();
        }

        public void SetupOnChangeHandlers()
        {
            _secretKeyGatewayNotifyer.NotifyGateway(GetAuthSettings, GetExternalHosts.GatewaySocket);

            _onChangeListenerDisposable = _settings.OnChange(accountSettings =>
            {
                if (CheckIfAuthSettingsAreChanged(accountSettings.Auth))
                {
                    _secretKeyGatewayNotifyer.NotifyGateway(accountSettings.Auth, GetExternalHosts.GatewaySocket);
                }
            });
        }

        private bool CheckIfAuthSettingsAreChanged(AuthValues updatedValues)
        {
            return GetAuthSettings.Issuer != updatedValues.Issuer 
                || GetAuthSettings.Audience != updatedValues.Audience
                || GetSecretKey != updatedValues.SecretKey;
        }
    }
}
