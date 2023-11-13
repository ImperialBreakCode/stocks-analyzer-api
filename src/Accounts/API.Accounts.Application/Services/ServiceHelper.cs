using API.Accounts.Application.DTOs;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services
{
    public static class ServiceHelper
    {
        public static string? GetUserWallet(IAccountsDbContext context, string username, out Wallet? wallet)
        {
            string? userId = context.Users.GetOneByUserName(username)?.Id;

            if (userId is null)
            {
                wallet = null;
                return ResponseMessages.UserNotFound;
            }

            wallet = context.Wallets.GetUserWallet(userId);
            return null;
        }
    }
}
