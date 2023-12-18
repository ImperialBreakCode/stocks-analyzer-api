using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/[controller]")]
	[ApiController]

	public class StockController : Controller
	{
		private readonly IStockService _stockService;

		public StockController(IStockService service)
		{
			_stockService = service;
		}

		[Authorize]
		[HttpGet]
		[Route("GetStock/{stockId}")]
		public async Task<IActionResult> GetStock(string stockId)
		{
			return await _stockService.GetStock(stockId);
		}

		[Authorize]
		[HttpGet]
		[Route("GetStocksInWallet/{walletId}")]
		public async Task<IActionResult> GetStocksInWallet(string walletId)
		{
			return await _stockService.GetStocksInWallet(walletId);
		}

		[Authorize]
		[HttpPut]
		[Route("AddStockForPurchase")]
		public async Task<IActionResult> AddStockForPurchase(StockDTO dto)
		{
			return await _stockService.AddStockForPurchase(dto);
		}

		[Authorize]
		[HttpPut]
		[Route("AddStockForSale")]
		public async Task<IActionResult> AddStockForSale(StockDTO dto)
		{
			return await _stockService.AddStockForPurchase(dto);
		}

		[Authorize]
		[HttpPost]
		[Route("ConfirmPurchase")]
		public async Task<IActionResult> ConfirmPurchase()
		{
			return await _stockService.ConfirmPurchase();
		}

		[Authorize]
		[HttpPost]
		[Route("ConfirmSale")]
		public async Task<IActionResult> ConfirmSale()
		{
			return await _stockService.ConfirmSale();
		}
	}
}
