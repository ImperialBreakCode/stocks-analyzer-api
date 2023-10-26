using API.Accounts.Application.DTOs;
using API.Accounts.Application.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register(RegisterLoginUserDTO userDTO)
        {
            _userService.RegisterUser(userDTO);
            return Ok();
        }

        //[HttpPost]
        //public IActionResult Login(RegisterLoginUserDTO userDTO)
        //{
        //    return Ok();
        //}

        //[HttpGet]
        //public IActionResult UserInformation()
        //{
        //    return Ok();
        //}
    }
}
