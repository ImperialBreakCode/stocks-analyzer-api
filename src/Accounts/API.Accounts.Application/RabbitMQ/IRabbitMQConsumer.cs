using RabbitMQ.Client.Events;

namespace API.Accounts.Application.RabbitMQ
{
    public interface IRabbitMQConsumer : IDisposable
    {
        void StartConsumer();
        void RegisterRecievedEvent(EventHandler<BasicDeliverEventArgs> eventHandler);
    }
}
