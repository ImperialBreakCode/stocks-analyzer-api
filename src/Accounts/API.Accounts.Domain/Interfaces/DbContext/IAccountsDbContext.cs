using API.Accounts.Domain.Entities;

namespace API.Accounts.Domain.Interfaces.DbContext
{
    public interface IAccountsDbContext : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<Stock> Stocks { get; }
        IWalletRepository Wallets { get; }
        IRepository<Transaction> Transactions { get; }

        void Commit();
    }
}
