using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalletController : Controller
	{
		private readonly IWalletService _walletService;

		public WalletController(IWalletService service)
		{
			_walletService = service;
		}

		[Authorize]
		[HttpPost]
		[Route("CreateWallet/{username}")]
		public async Task<IActionResult> CreateWallet(string username)
		{
			return await _walletService.CreateWallet(username);
		}

		[Authorize]
		[HttpGet]
		[Route("GetWallet/{walletId}")]
		public async Task<IActionResult> GetWallet(string walletId)
		{
			return await _walletService.GetWallet(walletId);
		}
	}
}
