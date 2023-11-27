using API.Accounts.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Analyzer.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Analyzer.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IAnalyzerStockService service;

        public StockController(IAnalyzerStockService service)
        {
            this.service = service;
        }

        [HttpGet("GetUserStocksInWallet/{walletId}")]
        public async Task<IActionResult> GetUserStocksInWallet(string walletId)
        {
                ICollection<GetStockResponseDTO>? jsonContent = await service.UserStocksInWallet(walletId);
                if (jsonContent != null)
                {
                    return Ok(jsonContent);
                }
                return StatusCode(500, "User profile not found");
            
        }

        //[HttpGet("percentage-change/{symbol}")]
        //public async Task<IActionResult> PercentageChange(string symbol)
        //{
        //    try
        //    {
        //        decimal percentageChange = await service.PercentageChange(symbol);
        //        return Ok(percentageChange);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Server error: {ex.Message}");
        //    }
        //}

    }
}
