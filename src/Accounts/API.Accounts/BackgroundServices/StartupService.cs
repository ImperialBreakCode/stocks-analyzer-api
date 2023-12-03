using API.Accounts.Application.Data;
using API.Accounts.Application.RabbitMQ;
using API.Accounts.Application.Settings;

namespace API.Accounts.BackgroundServices
{
    public class StartupService : BackgroundService
    {
        private readonly IAccountsSettingsManager _settingsManager;
        private readonly IRabbitMQSetupService _rabbitmqSetupService;
        private readonly IAccountsData _accountsData;

        public StartupService(
            IAccountsSettingsManager accountsSettingsManager,
            IAccountsData accountsData,
            IRabbitMQSetupService rabbitmqSetupService)
        {
            _settingsManager = accountsSettingsManager;
            _accountsData = accountsData;
            _rabbitmqSetupService = rabbitmqSetupService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _accountsData.EnsureDatabase();

            _settingsManager.SetupOnChangeHandlers();

            _rabbitmqSetupService.SetupAndStart();

            return Task.CompletedTask;
        }
    }
}
