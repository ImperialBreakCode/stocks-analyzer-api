using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public interface IUserRankManager
    {
         UserRank? GetUserType(Wallet? userWallet);
    }
}
