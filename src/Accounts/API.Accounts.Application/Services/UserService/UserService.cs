using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.UserService.EmailService;
using API.Accounts.Application.Services.UserService.UserRankService;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Application.Services.UserService
{
    internal class UserService : IUserService
    {
        private readonly IAccountsData _data;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenManager _tokenManager;
        private readonly IUserRankManager _userRankManager;
        private readonly IEmailConfirmation _emailConfirmation;

        public UserService(
            IAccountsData data, 
            IPasswordManager passwordManager, 
            ITokenManager tokenManager, 
            IUserRankManager userRankManager,
            IEmailConfirmation emailConfirmation
            )
        {
            _data = data;
            _passwordManager = passwordManager;
            _tokenManager = tokenManager;
            _userRankManager = userRankManager;
            _emailConfirmation = emailConfirmation;
        }

        public LoginResponseDTO LoginUser(LoginUserDTO loginDTO)
        {
            LoginResponseDTO responseDTO = new LoginResponseDTO();

            using(var context = _data.CreateDbContext())
            {
                User? user = context.Users.GetConfirmedByUserName(loginDTO.Username);

                if (user is null)
                {
                    responseDTO.Message = ResponseMessages.UserNotFound;
                }
                else if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
                {
                    responseDTO.Message = ResponseMessages.AuthPassIncorrect;
                }
                else
                {
                    responseDTO.Message = ResponseMessages.AuthSuccess;
                    responseDTO.Token = _tokenManager.CreateToken(loginDTO.Username, 60);
                }
            }

            return responseDTO;
        }

        public string? RegisterUser(RegisterUserDTO registerDTO)
        {
            string? result = null;

            using(var context = _data.CreateDbContext())
            {
                if (context.Users.GetOneByUserName(registerDTO.Username) is not null)
                {
                    return ResponseMessages.UserNameAlreadyExists;
                }

                if (context.Users.GetOneByEmail(registerDTO.Email) is not null)
                {
                    return ResponseMessages.UserEmailAlreadyExists;
                }

                User user = new()
                {
                    IsConfirmed = false,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                    PasswordHash = _passwordManager.HashPassword(registerDTO.Password, out string salt),
                    Salt = salt
                };

                context.Users.Insert(user);
                context.Commit();

                _emailConfirmation.SendEmail(user.Email, user.Id);
            }

            return result;
        }

        public GetUserResponseDTO? GetUserByUserName(string username)
        {
            GetUserResponseDTO? responseDTO = null;

            using (var context = _data.CreateDbContext())
            {
                User? user = context.Users.GetConfirmedByUserName(username);

                if (user is not null)
                {
                    Wallet? wallet = context.Wallets.GetUserWallet(user.Id);

                    responseDTO = new GetUserResponseDTO()
                    {
                        UserId = user.Id,
                        UserName = username,
                        UserRank = _userRankManager.GetUserType(wallet),
                        FirstName = user.FirstName,
                        UserEmail = user.Email,
                        LastName = user.LastName,
                        WalletId = wallet?.Id
                    };
                }
            }

            return responseDTO;
        }

        public string? UpdateUser(UpdateUserDTO updateDTO, string username)
        {
            using (var context = _data.CreateDbContext())
            {
                User? user = context.Users.GetConfirmedByUserName(username);
                if (user is null)
                {
                    return ResponseMessages.UserNotFound;
                }

                bool newUserNameExists = updateDTO.UserName != null
                    && user.UserName != updateDTO.UserName
                    && context.Users.GetOneByUserName(updateDTO.UserName) is not null;

                if (newUserNameExists)
                {
                    return ResponseMessages.UserNameAlreadyExists;
                }

                bool newEmailExists = updateDTO.Email != null
                    && user.Email != updateDTO.Email
                    && context.Users.GetOneByEmail(updateDTO.Email) is not null;

                if (newEmailExists)
                {
                    return ResponseMessages.UserEmailAlreadyExists;
                }

                user.UserName = updateDTO.UserName ?? user.UserName;
                user.FirstName = updateDTO.FirstName ?? user.FirstName;
                user.LastName = updateDTO.LastName ?? user.LastName;
                user.Email = updateDTO.Email ?? user.Email;

                context.Users.Update(user);
                context.Commit();
            }

            return null;
        }

        public void DeleteUser(string username)
        {
            using (var context = _data.CreateDbContext())
            {
                ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);

                if (wallet is not null)
                {
                    context.Wallets.DeleteWalletWithItsChildren(wallet.Id);
                }

                context.Users.DeleteByUserName(username);
                context.Commit();
            }
        }

        public bool ConfirmUser(string userId)
        {
            using (var context = _data.CreateDbContext())
            {
                var userRead = (IRepoRead<User>)context.Users;
                User? userForConfirmation = userRead.GetOneById(userId);

                bool confirmPass = userForConfirmation is not null && !userForConfirmation.IsConfirmed;

                if (confirmPass)
                {
                    userForConfirmation.IsConfirmed = true;

                    Wallet wallet = new Wallet()
                    {
                        Balance = 10000,
                        IsDemo = true,
                        UserId = userId,
                    };

                    context.Wallets.Insert(wallet);
                    context.Users.Update(userForConfirmation); 
                    context.Commit();
                }

                return confirmPass;
            }
        }
    }
}
