using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class StocksController : Controller
	{
		private readonly IStocksService _stocksService;

		public StocksController(IStocksService service)
		{
			_stocksService = service;
		}

		[HttpGet]
		[Route("current/{companyName}")]
		public async Task<IActionResult> GetCurrentData(string companyName)
		{
			var res = await _stocksService.GetCurrentData(companyName);

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("daily/{companyName}")]
		public async Task<IActionResult> GetDailyData(string companyName)
		{
			var res = await _stocksService.GetDailyData(companyName);

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("weekly/{companyName}")]
		public async Task<IActionResult> GetWeeklyData(string companyName)
		{
			var res = await _stocksService.GetWeeklyData(companyName);

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("monthly/{companyName}")]
		public async Task<IActionResult> GetMonthlyData(string companyName)
		{
			var res = await _stocksService.GetMonthlyData(companyName);

			return res;
		}
	}
}
