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
        private readonly ISecretKeyGatewayNotifyer _secretKeyGatewayNotifyer;

        public AccountSettingsAdapter(IOptionsMonitor<AccountSettings> settings, ISecretKeyGatewayNotifyer secretKeyGatewayNotifyer)
        {
            _settings = settings;
            _secretKeyGatewayNotifyer = secretKeyGatewayNotifyer;
        }

        public ICollection<string> GetAllowedHosts
            => _settings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts GetExternalHosts
            => _settings.CurrentValue.ExternalMicroservicesHosts;

        public string GetSecretKey
            => _settings.CurrentValue.SecretKey;

        public void Dispose()
        {
            _onChangeListenerDisposable?.Dispose();
        }

        public void SetupOnChangeHandlers()
        {
            _secretKeyGatewayNotifyer.NotifyGateway(GetSecretKey);

            _onChangeListenerDisposable = _settings.OnChange(accountSettings =>
            {
                _secretKeyGatewayNotifyer.NotifyGateway(accountSettings.SecretKey);
            });
        }
    }
}
