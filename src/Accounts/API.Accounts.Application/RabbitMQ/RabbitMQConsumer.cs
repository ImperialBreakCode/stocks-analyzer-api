using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace API.Accounts.Application.RabbitMQ
{
    internal class RabbitMQConsumer : IRabbitMQConsumer
    {
        private IConnection _connection;
        private IModel _channel;

        private EventingBasicConsumer _consumer;
        private ConnectionFactory _connectionFactory;
        private string _queueName;

        public RabbitMQConsumer(string hostName, string queueName)
        {
            _connectionFactory = new()
            {
                HostName = hostName
            };

            _queueName = queueName;
        }

        public void Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _consumer = new EventingBasicConsumer(_channel);
        }

        public void StartConsumer()
        {
            _channel.QueueDeclare(_queueName, exclusive: false);
            _channel.BasicConsume(_queueName, true, _consumer);
        }

        public void RegisterRecievedEvent(EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            _consumer.Received += eventHandler;
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
