using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    internal class UserRankManager : IUserRankManager
    {
        private decimal _specialTraderMin = 10000;
        private decimal _vipTraderMin = 100000;

        public UserRank? GetUserType(Wallet? userWallet)
        {
            if (userWallet is null)
            {
                return null;
            }

            if (userWallet.IsDemo)
            {
                return UserRank.Demo;
            }
            else if (userWallet.Balance >= _specialTraderMin)
            {
                return UserRank.SpecialTrader;
            }
            else if (userWallet.Balance >= _vipTraderMin)
            {
                return UserRank.VipTrader;
            }

            return UserRank.RegularTrader;
        }
    }
}
