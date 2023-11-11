﻿using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
			return await _accountService.Register(regUserDTO);

		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(UserDTO userDTO)
		{
			return await _accountService.Login(userDTO);
		}

		[Authorize]
		[HttpGet]
		[Route("UserInformation/{username}")]
		public async Task<IActionResult> UserInformation(string username)
		{
			return await _accountService.UserInformation(username);
		}
	}
}
