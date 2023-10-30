using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	public class AnalyzerController : Controller
	{
		
		public async Task<ActionResult> CurrentProfitability()
		{
			return Ok();
		}
		public async Task<ActionResult> PercentageChange()
		{
			return Ok();
		}
		public async Task<ActionResult> PortfolioRisk()
		{
			return Ok();
		}
		public async Task<ActionResult> DailyProfitabilityChanges()
		{
			return Ok();
		}

	}
}
