using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAccountsData _data;
        private readonly IPasswordManager _passwordManager;

        public UserService(IAccountsData data, IPasswordManager passwordManager)
        {
            _data = data;
            _passwordManager = passwordManager;
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
                    PasswordHash = _passwordManager.HashPassword(userDTO.Password, out string salt),
                    Salt = salt
                };

                context.Users.Insert(user);
                context.Commit();
            }

            return "token";
        }
    }
}
