using API.Accounts.Application.Settings;
using API.Accounts.Application.Settings.Options.AccountOptions;
using API.Accounts.Application.Settings.Options.DatabaseOptions;
using API.Accounts.Application.Settings.UpdateHandlers;
using Microsoft.Extensions.Options;

namespace API.Accounts.Implementations
{
    public class AccountSettingsAdapter : IAccountsSettingsManager
    {
        private readonly IOptionsMonitor<AccountSettings> _accountSettings;
        private readonly IOptions<DatabaseConnectionsSettings> _databaseConnections;

        private IDisposable? _onChangeListenerDisposable;
        private readonly IAuthTokenGatewayNotifyer _authTokenGatewayNotifyer;

        public AccountSettingsAdapter(IOptionsMonitor<AccountSettings> settings, IAuthTokenGatewayNotifyer authTokenGatewayNotifyer, IOptions<DatabaseConnectionsSettings> databaseConnections)
        {
            _accountSettings = settings;
            _authTokenGatewayNotifyer = authTokenGatewayNotifyer;
            _databaseConnections = databaseConnections;
        }

        public ICollection<string> AllowedHosts
            => _accountSettings.CurrentValue.AllowedHosts;

        public ExternalMicroservicesHosts ExternalHosts
            => _accountSettings.CurrentValue.ExternalMicroservicesHosts;

        public string SecretKey
            => _accountSettings.CurrentValue.Auth.SecretKey;

        public AuthValues AuthSettings
            => _accountSettings.CurrentValue.Auth;

        public EmailConfiguration EmailConfiguration 
            => _accountSettings.CurrentValue.EmailConfig;


        public string AccountDbConnection 
            => _databaseConnections.Value.AccountsDbContextConnection;

        public void Dispose()
        {
            _onChangeListenerDisposable?.Dispose();
        }

        public void SetupOnChangeHandlers()
        {
            _authTokenGatewayNotifyer.NotifyGateway(AuthSettings, ExternalHosts.GatewaySocket);

            _onChangeListenerDisposable = _accountSettings.OnChange(accountSettings =>
            {
                _authTokenGatewayNotifyer.NotifyGateway(accountSettings.Auth, ExternalHosts.GatewaySocket);
            });
        }
    }
}
