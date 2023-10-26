using API.Accounts.Application.Data;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Data
{
    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly IConfiguration _configuration;
        private IDbContextFactoryAdaptee _dbContext;

        public DataSourceFactory(IConfiguration configuration, IDbContextFactoryAdaptee dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public IAccountsDbContext Create()
        {
            string connectionString = _configuration.GetConnectionString("AccountsDbContextConnection");
            return _dbContext.CreateDbContext(connectionString);
        }
    }
}
