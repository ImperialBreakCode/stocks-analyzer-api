using API.Accounts.Application.Data;
using API.Accounts.Application.Data.ExchangeRates;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Application.Services.WalletService
{
    public class WalletService : IWalletService
    {
        private readonly IAccountsData _accountData;
        private readonly IExchangeRatesData _exchangeRatesData;

        public WalletService(IAccountsData accountData, IExchangeRatesData exchangeRatesData)
        {
            _accountData = accountData;
            _exchangeRatesData = exchangeRatesData;
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

                if (context.Wallets.GetUserWallet(user.Id) is not null)
                {
                    return ResponseMessages.WalletAlreadyExists;
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

        public string Deposit(DepositWalletDTO depositDTO)
        {
            using (var context = _accountData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(depositDTO.WalletId);

                if (wallet is null)
                {
                    return ResponseMessages.WalletNotFound;
                }
                else if (wallet.IsDemo)
                {
                    context.Wallets.DeleteWalletWithItsChildren(wallet.Id);
                    
                    wallet.IsDemo = false;
                    wallet.Balance = 0;
                    wallet.Id = Guid.NewGuid().ToString();

                    context.Wallets.Insert(wallet);
                }

                wallet.Balance += depositDTO.Value * _exchangeRatesData.GetRateToDollar(depositDTO.CurrencyType);

                context.Wallets.Update(wallet);
                context.Commit();
            }

            return string.Empty;
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
