using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.Mockup.MemoryData;
using API.Accounts.Infrastructure.Mockup.Repositories;

namespace API.Accounts.Infrastructure.Mockup.MemoryDbContext
{
    public class AccountMockupDbContext : IAccountsDbContext
    {

        private readonly IRepositoryFactory _repositoryFactory;

        private IUserRepository _users;
        private IRepository<Stock> _stocks;
        private IWalletRepository _wallets;
        private IRepository<Transaction> _transactions;

        private IMemoryData _memoryData;

        public AccountMockupDbContext(IDictionary<string, IDictionary<string, object>> data)
        {
            _memoryData = new MemoryDataMockup(data);
            _repositoryFactory = new MockupRepositoryFactory(_memoryData);
        }

        public IUserRepository Users 
            => _users ??= _repositoryFactory.CreateUserRepo();

        public IRepository<Stock> Stocks
            => _stocks ??= _repositoryFactory.CreateGenericRepo<Stock>();

        public IWalletRepository Wallets 
            => _wallets ??= _repositoryFactory.CreateWalletRepo();

        public IRepository<Transaction> Transactions
            => _transactions ??= _repositoryFactory.CreateGenericRepo<Transaction>();

        public void Commit()
        {
            _memoryData.SaveChanges();
        }

        public void Dispose()
        {
            //_memoryData.DiscardChanges();
        }
    }
}
