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

        public async Task RunClock()
        {
            while (await _periodicTimer.WaitForNextTickAsync())
            {
                _timeChangedEvent?.Invoke();
            }
        }

        public void Dispose()
        {
            _periodicTimer.Dispose();
        }
    }
}
