using API.Accounts.Application.Data;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Implementations
{
    public class AccountDataAdapter : IAccountsData
    {
        private readonly IConfiguration _configuration;
        private ISqlContextCreator _dbContext;

        public AccountDataAdapter(IConfiguration configuration, ISqlContextCreator dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public IAccountsDbContext CreateDbContext()
        {
            string connectionString = _configuration.GetConnectionString("AccountsDbContextConnection");
            return _dbContext.CreateDbContext(connectionString);
        }
    }
}
