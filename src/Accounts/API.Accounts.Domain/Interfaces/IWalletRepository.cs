using API.Accounts.Domain.Entities;

namespace API.Accounts.Domain.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Wallet? GetUserWallet(string userId);
    }
}
