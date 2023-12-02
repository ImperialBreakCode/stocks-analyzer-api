using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace API.Accounts.Application.RabbitMQ
{
    internal class RabbitMQConsumer : IRabbitMQConsumer
    {
        private IConnection _connection;
        private IModel _channel;

        private EventingBasicConsumer _consumer;
        private string _queueName;

        public RabbitMQConsumer(string hostName, string queueName)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _consumer = new EventingBasicConsumer(_channel);
            _queueName = queueName;            
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
