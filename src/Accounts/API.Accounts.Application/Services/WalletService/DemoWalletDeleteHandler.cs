using API.Accounts.Application.Data;
using API.Accounts.Application.Services.WalletService.Interfaces;

namespace API.Accounts.Application.Services.WalletService
{
    internal class DemoWalletDeleteHandler : IDemoWalletDeleteHandler
    {
        private readonly IAccountsData _accountsData;
        private readonly IWalletDeleteRabbitMQProducer _walletDeleteRabbitMQProducer;

        public DemoWalletDeleteHandler(IAccountsData accountsData, IWalletDeleteRabbitMQProducer walletDeleteRabbitMQProducer)
        {
            _accountsData = accountsData;
            _walletDeleteRabbitMQProducer = walletDeleteRabbitMQProducer;
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

                foreach (var walletId in expiredDemoWalletsIds)
                {
                    _walletDeleteRabbitMQProducer.SendWalletIdForDeletion(walletId);
                }
            }
        }
    }
}
