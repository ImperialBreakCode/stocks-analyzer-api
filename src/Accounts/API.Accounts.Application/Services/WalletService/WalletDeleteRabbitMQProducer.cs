using API.Accounts.Application.RabbitMQ.Interfaces;
using API.Accounts.Application.Services.WalletService.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace API.Accounts.Application.Services.WalletService
{
    public class WalletDeleteRabbitMQProducer : IWalletDeleteRabbitMQProducer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly IWaitingDeletedWalletIdsList _waitingIds;

        public WalletDeleteRabbitMQProducer(IRabbitMQConnection rabbitMQConnection, IWaitingDeletedWalletIdsList waitingDeletedWalletIdsList)
        {
            _waitingIds = waitingDeletedWalletIdsList;
            _rabbitMQConnection = rabbitMQConnection;
        }

        public void SendWaitingWalletIdsForDeletion()
        {
            foreach (var walletId in _waitingIds.WaitingIds)
            {
                SendWalletIdForDeletion(walletId);
            }
        }

        public void SendWalletIdForDeletion(string walletId)
        {
            if (!_rabbitMQConnection.IsConnected)
            {
                if (!_waitingIds.WaitingIds.Contains(walletId))
                {
                    _waitingIds.WaitingIds.Add(walletId);
                }
                
                return;
            }

            using (IModel channel = _rabbitMQConnection.CreateChannel())
            {
                string queueName = "deleteWallet";

                channel.QueueDeclare(queueName, durable: true, exclusive: false);
                var dataToSend = Encoding.UTF8.GetBytes(walletId);
                channel.BasicPublish("", queueName, body: dataToSend);
            }

            _waitingIds.WaitingIds.Remove(walletId);
        }
    }
}
