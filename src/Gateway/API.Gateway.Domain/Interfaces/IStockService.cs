using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IStockService
	{
		Task<IActionResult> GetStock(string stockId);
		Task<IActionResult> GetStocksInWallet(string walletId);
		Task<IActionResult> AddStockForPurchase(StockDTO dto);
		Task<IActionResult> AddStockForSale(StockDTO dto);
		Task<IActionResult> ConfirmPurchase(string walletId);
		Task<IActionResult> ConfirmSale(string walletId);
	}
}
