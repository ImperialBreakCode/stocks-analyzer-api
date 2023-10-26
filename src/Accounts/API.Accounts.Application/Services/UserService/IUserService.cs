using API.Accounts.Application.DTOs;

namespace API.Accounts.Application.Services.UserService
{
    public interface IUserService
    {
        string RegisterUser(RegisterLoginUserDTO userDTO);
        string LoginUser(RegisterLoginUserDTO userDTO);
        string GetUserByUserName(string username);
    }
}
