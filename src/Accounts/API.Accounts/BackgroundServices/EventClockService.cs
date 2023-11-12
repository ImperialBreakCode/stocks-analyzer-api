using API.Accounts.Application.EventClocks;
using API.Accounts.Application.Services.WalletService;

namespace API.Accounts.BackgroundServices
{
    public class EventClockService : IHostedService
    {
        private readonly IEventClock _eventClock;
        private readonly IDemoWalletDeleteHandler _deleteDemoWalletHandler;
        private Task _clockTask;

        public EventClockService(IEventClock eventClock, IDemoWalletDeleteHandler deleteDemoWalletHandler)
        {
            _eventClock = eventClock;
            _deleteDemoWalletHandler = deleteDemoWalletHandler;

            RegisterHandlers();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _clockTask = Task.Run(() => _eventClock.RunClock(), cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_clockTask != null && !_clockTask.IsCompleted)
            {
                try
                {
                    _eventClock.Dispose();
                    await Task.WhenAny(_clockTask, Task.Delay(Timeout.Infinite, cancellationToken));
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("clock stopped");
                }
            }
        }

        private void RegisterHandlers()
        {
            _eventClock.RegisterClockHandler(_deleteDemoWalletHandler.DeleteWallet);
        }
    }
}
