using API.Accounts.Application.Settings.GatewaySecretKeySender;
namespace API.Accounts.Application.Settings.GatewaySettingsSender
{
    public class SignalRGatewaySettingSender : IGatewaySettingsSender
    {
        public bool SendSecretKeyToGateway(string gatewayKey)
        {
            return true;
        }
    }
}
