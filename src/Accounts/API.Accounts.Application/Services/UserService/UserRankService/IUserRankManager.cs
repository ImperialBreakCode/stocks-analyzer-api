using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService.UserRankService
{
    public interface IUserRankManager
    {
        UserRank? GetUserType(Wallet? userWallet);
    }
}
