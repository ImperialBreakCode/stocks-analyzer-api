namespace API.Accounts.Application.EventClocks
{
    public class EventClock : IEventClock
    {
        private event Action _timeChangedEvent;
        private readonly PeriodicTimer _periodicTimer;

        public EventClock()
        {
            _periodicTimer = new PeriodicTimer(TimeSpan.FromHours(3));
        }

        public void RegisterClockHandler(Action handler)
        {
            _timeChangedEvent += handler;
        }

        public async Task RunClock(CancellationToken cancellationToken)
        {
            do
            {
                _timeChangedEvent?.Invoke();
            }
            while (await _periodicTimer.WaitForNextTickAsync() && !cancellationToken.IsCancellationRequested);
            
        }

        public void Dispose()
        {
            _periodicTimer.Dispose();
        }
    }
}
