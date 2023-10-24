using API.Accounts.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register(RegisterLoginUserDTO userDTO)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Login(RegisterLoginUserDTO userDTO)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult UserInformation()
        {
            return Ok();
        }
    }
}
