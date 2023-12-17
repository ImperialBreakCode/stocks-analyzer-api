using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces.Services;
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
		[HttpPut]
		[Route("Deposit")]
		public async Task<IActionResult> Deposit(DepositWalletDTO dto)
		{
			return await _walletService.Deposit(dto);
		}

		[Authorize]
		[HttpPost]
		[Route("CreateWallet")]
		public async Task<IActionResult> CreateWallet()
		{
			return await _walletService.CreateWallet();
		}

		[Authorize]
		[HttpDelete]
		[Route("DeleteWallet")]
		public async Task<IActionResult> DeleteWallet()
		{
			return await _walletService.DeleteWallet();
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
