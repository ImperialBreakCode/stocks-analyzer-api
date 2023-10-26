using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Data
{
    public interface IDbContextFactoryAdaptee
    {
        IAccountsDbContext CreateDbContext(string connectionString);
    }
}
