using API.Accounts.Application.Data;

namespace API.Accounts.Application.Services.WalletService
{
    public class DemoWalletDeleteHandler : IDemoWalletDeleteHandler
    {
        private readonly IAccountsData _accountsData;

        public DemoWalletDeleteHandler(IAccountsData accountsData)
        {
            _accountsData = accountsData;
        }

        public void DeleteWallet()
        {
            using (var context = _accountsData.CreateDbContext())
            {
                ICollection<string> expiredDemoWalletsIds = context.Wallets
                    .GetDemoWallets()
                    .Where(w => (w.CreatedAt - DateTime.UtcNow).Days > 60)
                    .Select(w => w.Id)
                    .ToList();

                foreach (var walletId in expiredDemoWalletsIds)
                {
                    context.Wallets.DeleteWalletWithItsChildren(walletId);
                }

                context.Commit();
            }
        }
    }
}
