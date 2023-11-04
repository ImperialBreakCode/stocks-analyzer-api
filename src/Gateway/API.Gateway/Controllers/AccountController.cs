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
			var response = await _accountService.Register(regUserDTO);

			return (IActionResult)response;
		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(UserDTO userDTO)
		{
			var response = await _accountService.Login(userDTO);

			return (IActionResult)response;
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
