using API.Accounts.Application.Data;
using API.Accounts.Application.Data.ExchangeRates;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.WalletService.Interfaces;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Application.Services.WalletService
{
    internal class WalletService : IWalletService
    {
        private readonly IAccountsData _accountData;
        private readonly IExchangeRatesData _exchangeRatesData;
        private readonly IWalletDeleteRabbitMQProducer _walletDeleteRabbitMQProducer;

        public WalletService(IAccountsData accountData, IExchangeRatesData exchangeRatesData, IWalletDeleteRabbitMQProducer walletDeleteRabbitMQProducer)
        {
            _accountData = accountData;
            _exchangeRatesData = exchangeRatesData;
            _walletDeleteRabbitMQProducer = walletDeleteRabbitMQProducer;
        }

        public string CreateWallet(string username)
        {
            using (var context = _accountData.CreateDbContext())
            {
                User? user = context.Users.GetConfirmedByUsername(username);

                if (user is null) 
                    return ResponseMessages.UserNotFound;
                

                if (context.Wallets.GetUserWallet(user.Id) is not null)
                    return ResponseMessages.WalletAlreadyExists;
                

                Wallet wallet = new Wallet()
                {
                    UserId = user.Id,
                };

                context.Wallets.Insert(wallet);
                context.Commit();
            }

            return ResponseMessages.WalletCreated;
        }

        public string DeleteWallet(string username)
        {
            using (var context = _accountData.CreateDbContext())
            {
                string? error = ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);
                if (error is not null)
                    return error;
                else if (wallet is null)
                    return ResponseMessages.WalletNotFound;

                context.Wallets.DeleteWalletWithItsChildren(wallet.Id);
                context.Commit();

                _walletDeleteRabbitMQProducer.SendWalletIdForDeletion(wallet.Id);
            }

            return ResponseMessages.WalletDeletedSuccessfully;
        }

        public string Deposit(DepositWalletDTO depositDTO, string username)
        {
            decimal exchangeRate;
            try
            {
                exchangeRate = _exchangeRatesData.GetRateToDollar(depositDTO.CurrencyType);
            }
            catch (ArgumentException)
            {
                return ResponseMessages.CannotDepositWithCurrencyType;
            }

            using (var context = _accountData.CreateDbContext())
            {
                string? error = ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);

                if (error is not null)
                {
                    return error;
                }
                else if (wallet is null)
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

                wallet.Balance += depositDTO.Value * exchangeRate;

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
                        IsDemo = wallet.IsDemo,
                        UserName = userRepo.GetOneById(wallet.UserId)!.UserName,
                    };
                }
            }

            return response;
        }
    }
}
