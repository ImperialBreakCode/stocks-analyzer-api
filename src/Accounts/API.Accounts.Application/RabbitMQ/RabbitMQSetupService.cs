using API.Accounts.Application.Services.TransactionService;
using RabbitMQ.Client.Exceptions;

namespace API.Accounts.Application.RabbitMQ
{
    public class RabbitMQSetupService : IRabbitMQSetupService
    {
        private readonly IRabbitMQConsumer _consumer;
        private readonly ITransactionSaleHandler _transactionSaleHandler;
        private bool _setupDelayed;

        public RabbitMQSetupService(IRabbitMQConsumer consumer, ITransactionSaleHandler transactionSaleHandler)
        {
            _consumer = consumer;
            _transactionSaleHandler = transactionSaleHandler;
            _setupDelayed = false;
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
                _consumer.Connect();
                _consumer.RegisterRecievedEvent(_transactionSaleHandler.HandleSale);
                _consumer.StartConsumer();
                _setupDelayed = false;
            }
            catch (BrokerUnreachableException)
            {
                _setupDelayed = true;
                Console.WriteLine("Failed to connect to RabbitMQ");
            }
        }
    }
}
