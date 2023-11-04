using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Application.Services.WalletService
{
    public class WalletService : IWalletService
    {
        private readonly IAccountsData _accountData;

        public WalletService(IAccountsData accountData)
        {
            _accountData = accountData;
        }

        public string CreateWallet(string username)
        {
            using (var context = _accountData.CreateDbContext())
            {
                User? user = context.Users.GetOneByUserName(username);

                if (user is null)
                {
                    return string.Format(ResponseMessages.UserNotFound, username);
                }

                Wallet wallet = new Wallet()
                {
                    UserId = user.Id,
                };

                context.Wallets.Insert(wallet);
                context.Commit();
            }

            return ResponseMessages.WalletCreated;
        }

        public GetWalletResponseDTO? GetWallet(string walletId)
        {
            GetWalletResponseDTO? response = null;

            using (var context = _accountData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(walletId);

                if (wallet is not null)
                {
                    var userRepo = (IRepoRead<User>)context.Users;
                    response = new GetWalletResponseDTO()
                    {
                        Id = wallet.Id,
                        Balance = wallet.Balance,
                        UserName = userRepo.GetOneById(wallet.UserId)!.UserName,
                    };
                }
            }

            return response;
        }
    }
}
