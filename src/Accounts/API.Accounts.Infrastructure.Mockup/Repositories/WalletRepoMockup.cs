using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Mockup.MemoryData;

namespace API.Accounts.Infrastructure.Mockup.Repositories
{
    public class WalletRepoMockup : RepoMockup<Wallet>, IWalletRepository
    {

        public WalletRepoMockup(IMemoryData memoryData) : base(memoryData)
        {
        }

        public void DeleteWalletWithItsChildren(string walletId)
        {
            Wallet? wallet = MemoryData.Get<Wallet>(walletId);

            if (wallet is not null)
            {
                var transactionIds = MemoryData.GetAll<Transaction>()
                    .Where(t => t.Walletid == wallet.Id)
                    .Select(t => t.Id);

                foreach (var transactionId in transactionIds)
                {
                    MemoryData.Delete<Transaction>(transactionId);
                }

                var stockIds = MemoryData.GetAll<Stock>()
                    .Where(s => s.WalletId == wallet.Id)
                    .Select(s => s.Id);

                foreach (var stockId in stockIds)
                {
                    MemoryData.Delete<Stock>(stockId);
                }

                Delete(walletId);
            }
        }

        public ICollection<Wallet> GetDemoWallets()
        {
            return GetManyByCondition(w => w.IsDemo);
        }

        public Wallet? GetUserWallet(string userId)
        {
            return GetManyByCondition(w => w.UserId == userId).FirstOrDefault();
        }
    }
}
