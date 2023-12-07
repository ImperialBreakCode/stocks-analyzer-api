using RabbitMQ.Client;

namespace API.Accounts.Application.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection : IDisposable
    {
        void Connect(string hostName);
        IModel CreateChannel();
    }
}
