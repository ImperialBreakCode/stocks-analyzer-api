using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Domain.Interfaces.Services
{
	public interface IWalletService
	{
		Task<IActionResult> CreateWallet();
		Task<IActionResult> Deposit(DepositWalletDTO dto);
		Task<IActionResult> DeleteWallet();
		Task<IActionResult> GetWallet(string walletId);

	}
}
