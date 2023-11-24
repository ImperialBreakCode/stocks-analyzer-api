using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings.GatewayAuthSettingsSender
{
    public interface ISocketGatewaySettingsSender
    {
        Task<bool> SendAuthTokenSettingsToGateway(AuthValues gatewayAuthValues, string gatewaySocketHost);
    }
}
