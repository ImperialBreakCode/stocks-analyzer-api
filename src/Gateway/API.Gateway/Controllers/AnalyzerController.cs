using API.Gateway.Domain.Interfaces;
using API.Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AnalyzerController : Controller
	{
		private readonly IAnalyzerService _analyzerService;
        public AnalyzerController(IAnalyzerService service)
        {
            _analyzerService = service;
        }

		[Authorize]
		[HttpGet]
		[Route("PortfolioSummary")]
		public async Task<IActionResult> PortfolioSummary()
		{
			var res = await _analyzerService.PortfolioRisk();

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("CurrentProfitability")]
		public async Task<IActionResult> CurrentProfitability()
		{
			var res = await _analyzerService.CurrentProfitability();

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("PercentageChange")]
		public async Task<IActionResult> PercentageChange()
		{
			var res = await _analyzerService.PercentageChange();

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("PortfolioRisk")]
		public async Task<IActionResult> PortfolioRisk()
		{
			var res = await _analyzerService.PortfolioRisk();

			return res;
		}
		[Authorize]
		[HttpGet]
		[Route("DailyProfitabilityChanges")]
		public async Task<IActionResult> DailyProfitabilityChanges()
		{
			var res = await _analyzerService.DailyProfitabilityChanges();

			return res;
		}

	}
}
