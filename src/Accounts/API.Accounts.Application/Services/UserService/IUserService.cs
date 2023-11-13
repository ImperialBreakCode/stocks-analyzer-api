using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.UserService
{
    public interface IUserService
    {
        string? RegisterUser(RegisterUserDTO registerDTO);
        LoginResponseDTO LoginUser(LoginUserDTO loginDTO, string secretKey);
        GetUserResponseDTO? GetUserByUserName(string username);
        string? UpdateUser(UpdateUserDTO updateDTO, string username);
        void DeleteUser(string username);
    }
}
