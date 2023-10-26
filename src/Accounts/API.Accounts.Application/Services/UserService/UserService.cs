using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IDataSourceFactory _data;

        public UserService(IDataSourceFactory data)
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
            using(var context = _data.Create())
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
