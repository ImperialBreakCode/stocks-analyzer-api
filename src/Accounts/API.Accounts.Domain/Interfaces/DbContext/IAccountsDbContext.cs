using API.Accounts.Domain.Entities;

namespace API.Accounts.Domain.Interfaces.DbContext
{
    public interface IAccountsDbContext : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<Stock> Stocks { get; }
        IRepository<Wallet> Wallets { get; }
        IRepository<Transaction> Transactions { get; }

        void Commit();
    }
}
