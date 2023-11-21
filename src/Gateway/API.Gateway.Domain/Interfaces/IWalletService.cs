using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IWalletService
	{
		Task<IActionResult> CreateWallet();
		//Task<IActionResult> GetWallet();
		Task<IActionResult> Deposit(DepositWalletDTO dto);
		Task<IActionResult> DeleteWallet();
	}
}
