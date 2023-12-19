using API.Analyzer.Domain.DTOs;
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
                return StatusCode(404, "User profile not found");
            
        }

        [HttpGet("CurrentProfitability/{username}/{symbol}/{type}")]
        public async Task<ActionResult<decimal?>> GetCurrentProfitability(string username, string symbol, string type)
        {
            try
            {
                decimal? currentProfitability = await service.GetCurrentProfitability(username,symbol,type);

                if (currentProfitability.HasValue)
                {
                    return Ok(currentProfitability.Value);
                }
                else
                {
                    return NotFound("Unable to calculate current profitability.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while calculating current profitability {ex.Message}");
            }
        }

        [HttpGet("PercentageChange/{username}/{symbol}/{type}")]
        public async Task<ActionResult<List<decimal>>> PersentageChange(string username, string symbol, string type)
        {
            try
            {
                List<decimal> investmentPercentageGains = await service.PersentageChange(username, symbol, type);

                if (investmentPercentageGains.Count > 0)
                {
                    return Ok(investmentPercentageGains);
                }
                else
                {
                    return NotFound("Unable to calculate investment percentage gains.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Eror while calculating the percentage change:{ex.Message}");
            }
        }


        [HttpGet("CalculateAverageProfitability/{username}/{symbol}/{type}")]
        public async Task<ActionResult<decimal?>> CalculateAverageProfitability(string username, string symbol, string type)
        {
            try
            {
                decimal? averageProfitability = await service.CalculateAverageProfitability(username, symbol, type);

                if (averageProfitability.HasValue)
                {
                    return Ok(averageProfitability.Value);
                }
                else
                {
                    return NotFound("Unable to calculate average profitability.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while calculating average profitability {ex.Message}");
            }
        }


    }
}
