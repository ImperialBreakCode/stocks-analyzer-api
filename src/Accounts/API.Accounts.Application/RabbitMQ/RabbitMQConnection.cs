using API.Accounts.Application.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace API.Accounts.Application.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private IConnection _connection;
        private bool _isConnected;

        public RabbitMQConnection()
        {
            _isConnected = false;
        }

        public bool IsConnected => _isConnected;

        public void Connect(string hostName)
        {
            if (!_isConnected)
            {
                ConnectionFactory connectionFactory = new ConnectionFactory()
                {
                    HostName = hostName,
                };

                _connection = connectionFactory.CreateConnection();
                _isConnected = true;
            }

        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
