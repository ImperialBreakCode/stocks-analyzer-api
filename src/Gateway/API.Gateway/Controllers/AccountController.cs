using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{

		private readonly IAccountService _accountService;

        public AccountController(IAccountService service)
        {
            _accountService = service;
        }


		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
		{
			await _accountService.Register(regUserDTO);


			return Ok();
		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(UserDTO userDTO)
		{
			LoginResponse response = await _accountService.Login(userDTO);

			if (response.Token == string.Empty)
				return BadRequest(response);

			return Ok(response);
		}
		[Authorize]
		[HttpPut]
		[Route("Deposit")]
		public async Task<IActionResult> Deposit()
		{
			return Ok();
		}
		[Authorize]
		[HttpPost]
		[Route("CreateWallet")]
		public async Task<IActionResult> CreateWallet()
		{
			return Ok();
		}
		[Authorize]
		[HttpGet]
		[Route("UserInformation")]
		public async Task<IActionResult> UserInformation()
		{
			return Ok();
		}
	}
}
