using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAccountsData _data;

        public UserService(IAccountsData data)
        {
            _data = data;
        }

        public string GetUserByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public string LoginUser(RegisterLoginUserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public string RegisterUser(RegisterLoginUserDTO userDTO)
        {
            // for testing purposes 
            using(var context = _data.CreateDbContext())
            {
                User user = new User()
                {
                    FirstName = "Lars",
                    LastName = "Owen",
                    UserName = userDTO.Username,
                    PasswordHash = userDTO.Password
                };

                context.Users.Insert(user);
                context.Commit();
            }

            return "token";
        }
    }
}
