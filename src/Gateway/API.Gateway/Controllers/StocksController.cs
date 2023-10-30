using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	public class StocksController : Controller
	{
		public async Task<ActionResult> BuyStock()
		{
			return Ok();
		}
		public async Task<ActionResult> Finalize()
		{
			return Ok();
		}
		public async Task<ActionResult> TrackStock()
		{
			return Ok();
		}
		public async Task<ActionResult> GetCurrentStocks()
		{
			return Ok();
		}
		public async Task<ActionResult> GetWeeklyStocks()
		{
			return Ok();
		}
		public async Task<ActionResult> GetMonthlyStocks()
		{
			return Ok();
		}
	}
}
