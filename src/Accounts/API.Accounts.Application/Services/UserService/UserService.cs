using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAccountsData _data;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenManager _tokenManager;

        public UserService(IAccountsData data, IPasswordManager passwordManager, ITokenManager tokenManager)
        {
            _data = data;
            _passwordManager = passwordManager;
            _tokenManager = tokenManager;
        }

        public LoginResponseDTO LoginUser(LoginUserDTO loginDTO, string secretKey)
        {
            LoginResponseDTO responseDTO = new LoginResponseDTO();

            using(var context = _data.CreateDbContext())
            {
                User? user = context.Users.GetOneByUserName(loginDTO.Username);

                if (user is null)
                {
                    responseDTO.Message = ResponseMessages.AuthUserNotFound;
                }
                else if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
                {
                    responseDTO.Message = ResponseMessages.AuthPassIncorrect;
                }
                else
                {
                    responseDTO.Message = ResponseMessages.AuthSuccess;
                    responseDTO.Token = _tokenManager.CreateToken(loginDTO.Username, 60, secretKey);
                }
            }

            return responseDTO;
        }

        public string? RegisterUser(RegisterUserDTO registerDTO)
        {
            string result = null;

            using(var context = _data.CreateDbContext())
            {
                if (context.Users.GetOneByUserName(registerDTO.Username) is not null)
                {
                    return ResponseMessages.UserAlreadyExists;
                }

                User user = new()
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                    PasswordHash = _passwordManager.HashPassword(registerDTO.Password, out string salt),
                    Salt = salt
                };

                Wallet wallet = new()
                {
                    Balance = 10000,
                    IsDemo = true,
                    UserId = user.Id
                };

                context.Users.Insert(user);
                context.Wallets.Insert(wallet);

                context.Commit();
            }

            return result;
        }

        public GetUserResponseDTO? GetUserByUserName(string username)
        {
            GetUserResponseDTO? responseDTO = null;

            using (var context = _data.CreateDbContext())
            {
                User? user = context.Users.GetOneByUserName(username);

                if (user is not null)
                {
                    responseDTO = new GetUserResponseDTO()
                    {
                        UserId = user.Id,
                        UserName = username,
                        FirstName = user.FirstName,
                        UserEmail = user.Email,
                        LastName = user.LastName,
                        WalletId = context.Wallets.GetUserWallet(user.Id)?.Id
                    };
                }
            }

            return responseDTO;
        }
    }
}
