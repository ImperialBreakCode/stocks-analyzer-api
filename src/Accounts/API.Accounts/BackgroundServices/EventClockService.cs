using API.Accounts.Application.EventClocks;
using API.Accounts.Application.Services.WalletService;
using API.Accounts.Application.Settings.UpdateHandlers;

namespace API.Accounts.BackgroundServices
{
    public class EventClockService : BackgroundService
    {
        private readonly IEventClock _eventClock;
        private readonly IDemoWalletDeleteHandler _deleteDemoWalletHandler;
        private readonly ISecretKeyGatewayNotifyer _secretKeyGatewayNotifyer;

        public EventClockService(
            IEventClock eventClock, 
            IDemoWalletDeleteHandler deleteDemoWalletHandler, 
            ISecretKeyGatewayNotifyer secretKeyGatewayNotifyer
            )
        {
            _eventClock = eventClock;
            _deleteDemoWalletHandler = deleteDemoWalletHandler;
            _secretKeyGatewayNotifyer = secretKeyGatewayNotifyer;

            RegisterHandlers();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventClock.RunClock(stoppingToken);
        }

        private void RegisterHandlers()
        {
            _eventClock.RegisterClockHandler(_deleteDemoWalletHandler.DeleteWallet);
            _eventClock.RegisterClockHandler(_secretKeyGatewayNotifyer.NotifyGateway);
        }
    }
}
