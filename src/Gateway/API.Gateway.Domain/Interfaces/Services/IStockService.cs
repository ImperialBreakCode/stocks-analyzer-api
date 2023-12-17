using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Domain.Interfaces.Services
{
	public interface IStockService
	{
		Task<IActionResult> GetStock(string stockId);
		Task<IActionResult> GetStocksInWallet(string walletId);
		Task<IActionResult> AddStockForPurchase(StockDTO dto);
		Task<IActionResult> AddStockForSale(StockDTO dto);
		Task<IActionResult> ConfirmPurchase();
		Task<IActionResult> ConfirmSale();
	}
}
