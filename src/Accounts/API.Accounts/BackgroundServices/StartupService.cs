using API.Accounts.Application.RabbitMQ;
using API.Accounts.Application.Services.TransactionService;
using API.Accounts.Application.Settings;

namespace API.Accounts.BackgroundServices
{
    public class StartupService : BackgroundService
    {
        private readonly IAccountsSettingsManager _settingsManager;
        private readonly IRabbitMQConsumer _consumer;
        private readonly ITransactionSaleHandler _transactionSaleHandler;

        public StartupService(
            IAccountsSettingsManager accountsSettingsManager,
            IRabbitMQConsumer rabbitMQConsumer,
            ITransactionSaleHandler transactionSaleHandler
            )
        {
            _consumer = rabbitMQConsumer;
            _transactionSaleHandler = transactionSaleHandler;
            _settingsManager = accountsSettingsManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _settingsManager.SetupOnChangeHandlers();

            _consumer.RegisterRecievedEvent(_transactionSaleHandler.HandleSale);
            _consumer.StartConsumer();

            return Task.CompletedTask;
        }
    }
}
