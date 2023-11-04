using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	[ApiController]
	[Route("api/StockInfo/{dataType}/{companyName}")]
	public class StocksController : Controller
	{
		private readonly IStocksService _stocksService;

		public StocksController(IStocksService service)
		{
			_stocksService = service;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetStockData(string dataType, string companyName)
		{
			var res = await _stocksService.GetStockData(dataType,companyName);

			if (res.IsSuccessStatusCode)
				return Ok(res);
			else 
				return BadRequest(res);
		}
	}
}
