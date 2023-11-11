using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Application.Settings;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountsSettingsManager _settingsManager;

        public UserController(IUserService userService, IAccountsSettingsManager settingsManager)
        {
            _userService = userService;
            _settingsManager = settingsManager;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterUserDTO userDTO)
        {
            _userService.RegisterUser(userDTO);
            return Created($"/api/User/{userDTO.Username}", userDTO);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUserDTO userDTO)
        {
            var response = _userService.LoginUser(userDTO, _settingsManager.GetSecretKey);

            if (response.Message == ResponseMessages.AuthSuccess)
            {
                return Ok(response);
            }

            return Unauthorized(response);
        }

        [HttpGet]
        [Route("UserInformation/{username}")]
        public IActionResult UserInformation(string username)
        {
            GetUserResponseDTO? userDto = _userService.GetUserByUserName(username);

            if (userDto is null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }
    }
}
