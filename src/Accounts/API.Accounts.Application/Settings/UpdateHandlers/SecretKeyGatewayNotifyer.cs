using API.Accounts.Application.Settings.GatewaySecretKeySender;

namespace API.Accounts.Application.Settings.UpdateHandlers
{
    public class SecretKeyGatewayNotifyer : ISecretKeyGatewayNotifyer
    {
        private string? _waitingSecretKey;
        private readonly IGatewaySettingsSender _settingsSender;

        public SecretKeyGatewayNotifyer(IGatewaySettingsSender gatewaySettings)
        {
            _settingsSender = gatewaySettings;
        }

        public void NotifyGateway()
        {
            if (_waitingSecretKey is not null)
            {
                NotifyGateway(_waitingSecretKey);
            }
        }

        public void NotifyGateway(string secretKey)
        {
            if (!_settingsSender.SendSecretKeyToGateway(secretKey))
            {
                _waitingSecretKey = secretKey;
            }
            else
            {
                _waitingSecretKey = null;
            }
        }
    }
}
