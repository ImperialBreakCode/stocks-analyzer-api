namespace API.Accounts.Application.EventClocks
{
    public interface IEventClock : IDisposable
    {
        void RegisterClockHandler(Action handler);
        Task RunClock();
    }
}
