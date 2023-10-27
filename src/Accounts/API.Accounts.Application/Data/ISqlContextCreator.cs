using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Data
{
    public interface ISqlContextCreator
    {
        IAccountsDbContext CreateDbContext(string connectionString);
    }
}
