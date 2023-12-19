using API.Accounts.Application.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace API.Accounts.Application.RabbitMQ
{
    internal class RabbitMQConsumer : IRabbitMQConsumer
    {
        private IModel _channel;

        private readonly IRabbitMQConnection _connection;
        private readonly EventingBasicConsumer _consumer;
        private readonly string _queueName;

        public RabbitMQConsumer(IRabbitMQConnection rabbitMQConnection, string queueName)
        {
            _connection = rabbitMQConnection;
            _consumer = new EventingBasicConsumer(_channel);
            _queueName = queueName;
        }

        public void StartConsumer()
        {
            _channel = _connection.CreateChannel();
            _channel.QueueDeclare(_queueName, exclusive: false, durable: true);
            _channel.BasicConsume(_queueName, true, _consumer);
        }

        public void RegisterRecievedEvent(EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            _consumer.Received += eventHandler;
        }

        public void Dispose()
        {
            _channel?.Close();
        }
    }
}
