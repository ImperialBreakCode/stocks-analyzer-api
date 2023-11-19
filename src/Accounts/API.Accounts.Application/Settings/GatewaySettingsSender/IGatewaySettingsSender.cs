namespace API.Accounts.Application.Settings.GatewaySecretKeySender
{
    public interface IGatewaySettingsSender
    {
        bool SendSecretKeyToGateway(string gatewayKey);
    }
}
