using API.Accounts.Application.Data;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Domain.Interfaces.DbManager;

namespace API.Accounts.Implementations
{
    public class AccountDataAdapter : IAccountsData
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlContextCreator _dbContext;
        private readonly IAccountsDbManager _dbManager;

        public AccountDataAdapter(IConfiguration configuration, ISqlContextCreator dbContext, IAccountsDbManager accountsDbManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _dbManager = accountsDbManager;
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
    }
}
