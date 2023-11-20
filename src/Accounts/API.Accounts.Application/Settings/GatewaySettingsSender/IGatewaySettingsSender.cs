using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings.GatewayAuthSettingsSender
{
    public interface IGatewaySettingsSender
    {
        Task<bool> SendAuthTokenSettingsToGateway(AuthValues gatewayKey, string gatewaySocketHost);
    }
}
