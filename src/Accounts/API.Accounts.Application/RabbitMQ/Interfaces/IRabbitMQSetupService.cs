namespace API.Accounts.Application.RabbitMQ.Interfaces
{
    public interface IRabbitMQSetupService
    {
        bool SetupDelayed { get; }
        void SetupAndStart();
        void DelayedSetupHandler();
    }
}
