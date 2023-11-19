namespace API.Accounts.Application.Settings.UpdateHandlers
{
    public class SecretKeyGatewayNotifyer : ISecretKeyGatewayNotifyer
    {
        private string? _waitingSecretKey;

        public void NotifyGateway()
        {
            if (_waitingSecretKey is not null)
            {
                NotifyGateway(_waitingSecretKey);
            }
        }

        public void NotifyGateway(string secretKey)
        {
            // if gateway is not avalibale add the secretKey as waiting (_waiting secret key)

            Console.WriteLine("notify gateway");
        }
    }
}
