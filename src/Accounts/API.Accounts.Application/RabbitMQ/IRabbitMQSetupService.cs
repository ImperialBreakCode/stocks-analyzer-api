namespace API.Accounts.Application.RabbitMQ
{
    public interface IRabbitMQSetupService
    {
        bool SetupDelayed { get; }
        void SetupAndStart();
        void DelayedSetupHandler();
    }
}
