using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    internal class UserTypeManager : IUserTypeManager
    {
        private decimal _specialTraderMin = 10000;
        private decimal _vipTraderMin = 100000;

        public UserType? GetUserType(Wallet? userWallet)
        {
            if (userWallet is null)
            {
                return null;
            }

            if (userWallet.IsDemo)
            {
                return UserType.Demo;
            }
            else if (userWallet.Balance >= _specialTraderMin)
            {
                return UserType.SpecialTrader;
            }
            else if (userWallet.Balance >= _vipTraderMin)
            {
                return UserType.VipTrader;
            }

            return UserType.RegularTrader;
        }
    }
}
