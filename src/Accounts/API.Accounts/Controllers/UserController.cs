using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _conf;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _conf = configuration;
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
            var response = _userService.LoginUser(userDTO, _conf.GetSection("Secrets")["SecretKey"]);

            if (response.Message == ResponseMessages.AuthSuccess)
            {
                return Ok(response);
            }

            return Unauthorized(response);
        }

        [HttpGet]
        public IActionResult UserInformation(string username)
        {
            return Ok();
        }
    }
}
