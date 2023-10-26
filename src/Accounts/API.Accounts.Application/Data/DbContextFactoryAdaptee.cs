using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.DbContext;

namespace API.Accounts.Application.Data
{
    public class DbContextFactoryAdaptee : IDbContextFactoryAdaptee
    {
        public IAccountsDbContext CreateDbContext(string connectionString)
        {
            return new AccountsDbContext(connectionString);
        }
    }
}
