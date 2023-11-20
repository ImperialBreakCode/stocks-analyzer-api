using API.Accounts.Application.Settings.GatewaySecretKeySender;
using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings.UpdateHandlers
{
    public class AuthTokenGatewayNotifyer : IAuthTokenGatewayNotifyer
    {
        private readonly IGatewaySettingsSender _settingsSender;
        private AuthValues? _waitingAuthTokenData;
        private string _gatwaySocketHost; 

        public AuthTokenGatewayNotifyer(IGatewaySettingsSender gatewaySettings)
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
                Console.WriteLine("added for waiting");
                _waitingAuthTokenData = authData;
            }
            else
            {
                _waitingAuthTokenData = null;
            }
        }
    }
}
