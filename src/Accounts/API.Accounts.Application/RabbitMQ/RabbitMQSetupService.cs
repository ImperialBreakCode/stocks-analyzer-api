using API.Accounts.Application.RabbitMQ.Interfaces;
using API.Accounts.Application.Services.TransactionService;
using RabbitMQ.Client.Exceptions;

namespace API.Accounts.Application.RabbitMQ
{
    public class RabbitMQSetupService : IRabbitMQSetupService
    {
        private const string _hostName  = "localhost";

        private readonly IRabbitMQConnection _connection;
        private readonly ICollection<IRabbitMQConsumer> _consumers;

        private readonly ITransactionSaleHandler _transactionSaleHandler;

        private bool _setupDelayed;

        public RabbitMQSetupService(ITransactionSaleHandler transactionSaleHandler, IRabbitMQConnection rabbitMQConnection)
        {
            _connection = rabbitMQConnection;
            _transactionSaleHandler = transactionSaleHandler;

            _consumers = new List<IRabbitMQConsumer>();
            _setupDelayed = false;

            AddConsumers();
        }

        public bool SetupDelayed => _setupDelayed;

        public void DelayedSetupHandler()
        {
            if (_setupDelayed)
            {
                SetupAndStart();
            }
        }

        public void SetupAndStart()
        {
            try
            {
                _connection.Connect(_hostName);

                foreach (var consumer in _consumers)
                {
                    consumer.StartConsumer();
                }

                _setupDelayed = false;
            }
            catch (BrokerUnreachableException)
            {
                _setupDelayed = true;
                Console.WriteLine("Failed to connect to RabbitMQ");
            }
        }

        private void AddConsumers()
        {
            var transactionSellStockConsumer = new RabbitMQConsumer(_connection, "transactionSellStock");
            transactionSellStockConsumer.RegisterRecievedEvent(_transactionSaleHandler.HandleSale);

            _consumers.Add(transactionSellStockConsumer);
        }
    }
}
