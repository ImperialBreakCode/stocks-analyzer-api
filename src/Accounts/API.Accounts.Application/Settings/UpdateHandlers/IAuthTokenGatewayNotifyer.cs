using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings.UpdateHandlers
{
    public interface IAuthTokenGatewayNotifyer
    {
        void NotifyGateway();
        void NotifyGateway(AuthValues authData, string gatewaySocketHost);
    }
}
