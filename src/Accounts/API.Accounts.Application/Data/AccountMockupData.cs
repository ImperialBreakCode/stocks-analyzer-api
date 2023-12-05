using API.Accounts.Application.Data.AccountsDataSeeder;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.Mockup.MemoryData;
using API.Accounts.Infrastructure.Mockup.MemoryDbContext;

namespace API.Accounts.Application.Data
{
    public class AccountMockupData : IAccountsData
    {
        private readonly IDictionary<string, IDictionary<string, object>> _data;
        private readonly IAccountsDataSeeder _accountsDataSeeder;

        public AccountMockupData(IAccountsDataSeeder accountsDataSeeder)
        {
            _data = new Dictionary<string, IDictionary<string, object>>();
            _accountsDataSeeder = accountsDataSeeder;
        }

        public IAccountsDbContext CreateDbContext()
        {
            return new AccountMockupDbContext(_data);
        }

        public void EnsureDatabase()
        {
            MemoryDataHelper.AssignMemoryTables(_data);
        }

        public void SeedData()
        {
            var context = CreateDbContext();
            _accountsDataSeeder.SeedData(context);
            context.Dispose();
        }
    }
}
