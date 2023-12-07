using API.Accounts.Application.RabbitMQ.Interfaces;
using API.Accounts.Application.Services.WalletService.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace API.Accounts.Application.Services.WalletService
{
    public class WalletDeleteRabbitMQProducer : IWalletDeleteRabbitMQProducer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;

        public WalletDeleteRabbitMQProducer(IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
        }

        public void SendWalletIdForDeletion(string walletId)
        {
            using (IModel channel = _rabbitMQConnection.CreateChannel())
            {
                string queueName = "deleteWallet";

                channel.QueueDeclare(queueName, durable: true, exclusive: false);
                var dataToSend = Encoding.UTF8.GetBytes(walletId);
                channel.BasicPublish("", queueName, body: dataToSend);
            }
        }
    }
}
