using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/[controller]")]
	[ApiController]

	public class StockInfoController : Controller
	{
		private readonly IStockInfoService _stocksService;

		public StockInfoController(IStockInfoService service)
		{
			_stocksService = service;
		}

		[HttpGet]
		[Route("current/{companyName}")]
		public async Task<IActionResult> GetCurrentData(string companyName)
		{
			return await _stocksService.GetCurrentData(companyName);
		}

		[Authorize]
		[HttpGet]
		[Route("daily/{companyName}")]
		public async Task<IActionResult> GetDailyData(string companyName)
		{
			return await _stocksService.GetDailyData(companyName);
		}

		[Authorize]
		[HttpGet]
		[Route("weekly/{companyName}")]
		public async Task<IActionResult> GetWeeklyData(string companyName)
		{
			return await _stocksService.GetWeeklyData(companyName);
		}

		[Authorize]
		[HttpGet]
		[Route("monthly/{companyName}")]
		public async Task<IActionResult> GetMonthlyData(string companyName)
		{
			return await _stocksService.GetMonthlyData(companyName);
		}
	}
}
