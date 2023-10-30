using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{

		public IAccountService _accountService { get; set; }

        public AccountController(IAccountService service)
        {
            _accountService = service;
        }


		[HttpPost]
		[Route("Register")]
		public async Task<ActionResult> Register(RegisterUserDTO regUserDTO)
		{
			var response = await _accountService.Register(regUserDTO);

			return (ActionResult)response;
		}
		[HttpPost]
		[Route("Login")]
		public async Task<ActionResult> Login(UserDTO userDTO)
		{
			var response = await _accountService.Login(userDTO);

			return (ActionResult)response;
		}
		[HttpPut]
		[Route("Deposit")]
		public async Task<ActionResult> Deposit()
		{
			return Ok();
		}
		[HttpPost]
		[Route("CreateWallet")]
		public async Task<ActionResult> CreateWallet()
		{
			return Ok();
		}
		[HttpGet]
		[Route("UserInformation")]
		public async Task<ActionResult> UserInformation()
		{
			return Ok();
		}
	}
}
