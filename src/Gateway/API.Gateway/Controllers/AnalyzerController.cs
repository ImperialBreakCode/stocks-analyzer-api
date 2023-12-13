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
			return await _analyzerService.PortfolioRisk();
		}

		[Authorize]
		[HttpGet]
		[Route("CurrentProfitability")]
		public async Task<IActionResult> CurrentProfitability()
		{
			return await _analyzerService.CurrentProfitability();
		}

		[Authorize]
		[HttpGet]
		[Route("PercentageChange")]
		public async Task<IActionResult> PercentageChange()
		{
			return await _analyzerService.PercentageChange();
		}

		[Authorize]
		[HttpGet]
		[Route("PortfolioRisk")]
		public async Task<IActionResult> PortfolioRisk()
		{
			return await _analyzerService.PortfolioRisk();
		}

		[Authorize]
		[HttpGet]
		[Route("DailyProfitabilityChanges")]
		public async Task<IActionResult> DailyProfitabilityChanges()
		{
			return await _analyzerService.DailyProfitabilityChanges();
		}

	}
}
