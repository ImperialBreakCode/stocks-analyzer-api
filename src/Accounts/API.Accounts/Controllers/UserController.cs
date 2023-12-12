using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Extensions;
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
        [Route("Register")]
        public IActionResult Register(RegisterUserDTO userDTO)
        {
            var errorMessage = _userService.RegisterUser(userDTO);

            if (errorMessage is not null)
            {
                return this.ParseAndReturnMessage(errorMessage);
            }

            return Created($"/api/User/{userDTO.Username}", userDTO);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUserDTO userDTO)
        {
            var response = _userService.LoginUser(userDTO);

            if (response.Message == ResponseMessages.AuthSuccess)
            {
                return Ok(response);
            }

            return Unauthorized(response);
        }

        [HttpGet]
        [Route("ConfirmUser/{userId}")]
        public IActionResult ConfirmUser(string userId)
        {
            if (!_userService.ConfirmUser(userId))
            {
                return BadRequest();
            }
            return Ok("Account is confirmed");
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

        [HttpPut]
        [Route("UpdateUser/{username}")]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO updateUserDTO, [FromRoute] string username)
        {
            string response = _userService.UpdateUser(updateUserDTO, username);
            
            return this.ParseAndReturnMessage(response);
        }

        [HttpDelete]
        [Route("DeleteUser/{username}")]
        public IActionResult DeleteUser(string username)
        {
            _userService.DeleteUser(username);
            return Ok();
        }
    }
}
