using API.Accounts.Domain.Entities;

namespace API.Accounts.Domain.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Wallet? GetUserWallet(string userId);
        public ICollection<Wallet> GetDemoWallets();
        void DeleteWalletWithItsChildren(string walletId);
    }
}
