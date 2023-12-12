using RabbitMQ.Client.Events;

namespace API.Accounts.Application.RabbitMQ.Interfaces
{
    public interface IRabbitMQConsumer : IDisposable
    {
        void StartConsumer();
        void RegisterRecievedEvent(EventHandler<BasicDeliverEventArgs> eventHandler);
    }
}
