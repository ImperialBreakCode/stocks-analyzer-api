using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.UserService
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDTO registerDTO);
        LoginResponseDTO LoginUser(LoginUserDTO loginDTO, string secretKey);
        string GetUserByUserName(string username);
    }
}
