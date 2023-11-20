using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public interface IUserTypeManager
    {
         UserType? GetUserType(Wallet? userWallet);
    }
}
