using API.Gateway.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
		[Route("PortfolioSummary/{walletId}")]
		public async Task<IActionResult> PortfolioSummary(string walletId)
		{
			return await _analyzerService.PortfolioSummary(walletId);
		}

		[Authorize]
		[HttpGet]
		[Route("CurrentBalanceInWallet/{walletId}")]
		public async Task<IActionResult> CurrentBalanceInWallet(string walletId)
		{
			return await _analyzerService.CurrentBalanceInWallet(walletId);
		}

		[Authorize]
		[HttpGet]
		[Route("GetUserStocksInWallet/{walletId}")]
		public async Task<IActionResult> GetUserStocksInWallet(string walletId)
		{
			return await _analyzerService.GetUserStocksInWallet(walletId);
		}

		[Authorize]
		[HttpGet]
		[Route("CurrentProfitability/{username}/{symbol}/{type}")]
		public async Task<IActionResult> CurrentProfitability(string username, string symbol, string type)
		{
			return await _analyzerService.CurrentProfitability(username, symbol, type);
		}

		[Authorize]
		[HttpGet]
		[Route("PercentageChange/{username}/{symbol}/{type}")]
		public async Task<IActionResult> PercentageChange(string username, string symbol, string type)
		{
			return await _analyzerService.PercentageChange(username, symbol, type);
		}

		[Authorize]
		[HttpGet]
		[Route("CalculateAverageProfitability/{username}/{symbol}/{type}")]
		public async Task<IActionResult> CalculateAverageProfitability(string username, string symbol, string type)
		{
			return await _analyzerService.CalculateAverageProfitability(username, symbol, type);
		}
	}
}
