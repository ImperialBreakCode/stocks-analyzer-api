using API.Accounts.Application.Data;
using API.Accounts.Application.Data.AccountsDataSeeder;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Domain.Interfaces.DbManager;

namespace API.Accounts.Implementations
{
    public class AccountDataAdapter : IAccountsData
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlContextCreator _dbContext;
        private readonly IAccountsDbManager _dbManager;
        private readonly IAccountsDataSeeder _accountsDataSeeder;

        public AccountDataAdapter(
            IConfiguration configuration, 
            ISqlContextCreator dbContext, 
            IAccountsDbManager accountsDbManager, 
            IAccountsDataSeeder accountsDataSeeder)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _dbManager = accountsDbManager;
            _accountsDataSeeder = accountsDataSeeder;
        }

        public IAccountsDbContext CreateDbContext()
        {
            string connectionString = _configuration.GetConnectionString("AccountsDbContextConnection");
            return _dbContext.CreateDbContext(connectionString);
        }

        public void EnsureDatabase()
        {
            string connectionString = _configuration.GetConnectionString("AccountsDbContextConnection");
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
