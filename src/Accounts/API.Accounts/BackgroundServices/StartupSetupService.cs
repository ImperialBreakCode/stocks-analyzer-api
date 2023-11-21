using API.Accounts.Application.Settings;

namespace API.Accounts.BackgroundServices
{
    public class StartupSetupService : BackgroundService
    {
        private readonly IAccountsSettingsManager _settingsManager;

        public StartupSetupService(IAccountsSettingsManager accountsSettingsManager)
        {
            _settingsManager = accountsSettingsManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _settingsManager.SetupOnChangeHandlers();

            return Task.CompletedTask;
        }
    }
}
