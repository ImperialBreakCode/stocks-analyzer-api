using API.Accounts.Application.EventClocks;
using API.Accounts.Application.Services.WalletService;

namespace API.Accounts.BackgroundServices
{
    public class EventClockService : BackgroundService
    {
        private readonly IEventClock _eventClock;
        private readonly IDemoWalletDeleteHandler _deleteDemoWalletHandler;

        public EventClockService(IEventClock eventClock, IDemoWalletDeleteHandler deleteDemoWalletHandler)
        {
            _eventClock = eventClock;
            _deleteDemoWalletHandler = deleteDemoWalletHandler;

            RegisterHandlers();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventClock.RunClock(stoppingToken);
        }

        private void RegisterHandlers()
        {
            _eventClock.RegisterClockHandler(_deleteDemoWalletHandler.DeleteWallet);
        }
    }
}
