using API.Accounts.Application.Data.AccountsDataSeeder;
using API.Accounts.Application.Settings;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Domain.Interfaces.DbManager;
using API.Accounts.Infrastructure.DbContext;

namespace API.Accounts.Application.Data
{
    public class AccountData : IAccountsData
    {
        private readonly IAccountsSettingsManager _settings;
        private readonly IAccountsDbManager _dbManager;
        private readonly IAccountsDataSeeder _accountsDataSeeder;

        public AccountData(
            IAccountsSettingsManager settings,
            IAccountsDbManager accountsDbManager,
            IAccountsDataSeeder accountsDataSeeder)
        {
            _settings = settings;
            _dbManager = accountsDbManager;
            _accountsDataSeeder = accountsDataSeeder;
        }

        public IAccountsDbContext CreateDbContext()
        {
            string connectionString = _settings.AccountDbConnection;
            return new AccountsDbContext(connectionString);
        }

        public void EnsureDatabase()
        {
            string connectionString = _settings.AccountDbConnection;
            _dbManager.EnsureDatabaseTables(connectionString);
        }

        public void SeedData()
        {
            var context = CreateDbContext();
            _accountsDataSeeder.SeedData(context);
            context.Dispose();
        }
    }
}
