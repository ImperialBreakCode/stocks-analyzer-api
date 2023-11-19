namespace API.Accounts.Application.Settings.UpdateHandlers
{
    public interface ISecretKeyGatewayNotifyer
    {
        void NotifyGateway();
        void NotifyGateway(string secretKey);
    }
}
