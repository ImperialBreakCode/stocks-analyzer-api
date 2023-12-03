using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.Mockup.MemoryData;
using API.Accounts.Infrastructure.Mockup.MemoryDbContext;

namespace API.Accounts.Application.Data
{
    public class AccountMockupData : IAccountsData
    {
        private readonly IDictionary<string, IDictionary<string, object>> _data;

        public AccountMockupData()
        {
            _data = new Dictionary<string, IDictionary<string, object>>();
        }

        public IAccountsDbContext CreateDbContext()
        {
            return new AccountMockupDbContext(_data);
        }

        public void EnsureDatabase()
        {
            MemoryDataHelper.AssignMemoryTables(_data);
        }
    }
}
