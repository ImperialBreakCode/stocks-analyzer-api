using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Data.AccountsDataSeeder
{
    public interface IAccountsDataSeeder
    {
        public void SeedData(IAccountsDbContext context);
    }
}
