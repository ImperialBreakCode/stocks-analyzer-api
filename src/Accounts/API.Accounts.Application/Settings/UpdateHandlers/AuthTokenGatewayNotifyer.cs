using API.Accounts.Application.Settings.GatewayAuthSettingsSender;
using API.Accounts.Application.Settings.Options.AccountOptions;

namespace API.Accounts.Application.Settings.UpdateHandlers
{
    internal class AuthTokenGatewayNotifyer : IAuthTokenGatewayNotifyer
    {
        private readonly ISocketGatewaySettingsSender _settingsSender;
        private AuthValues? _waitingAuthTokenData;
        private string _gatwaySocketHost; 

        public AuthTokenGatewayNotifyer(ISocketGatewaySettingsSender gatewaySettings)
        {
            _settingsSender = gatewaySettings;
        }

        public void NotifyGateway()
        {
            if (_waitingAuthTokenData is not null && _gatwaySocketHost is not null)
            {
                NotifyGateway(_waitingAuthTokenData, _gatwaySocketHost);
            }
        }

        public void NotifyGateway(AuthValues authData, string gatewaySocketHost)
        {
            _gatwaySocketHost = gatewaySocketHost;

            if (!_settingsSender.SendAuthTokenSettingsToGateway(authData, _gatwaySocketHost).Result)
            {
                _waitingAuthTokenData = authData;
            }
            else
            {
                _waitingAuthTokenData = null;
            }
        }
    }
}
