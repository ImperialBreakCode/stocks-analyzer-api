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
                return StatusCode(500, "User profile not found");
            
        }

        [HttpGet("PercentageChange/{walletId}")]
        public async Task<ActionResult<decimal?>> GetInvestmentPercentageChange(string walletId)
        {
            try
            {
                decimal? shareValue = await service.GetShareValue(walletId);

                if (shareValue.HasValue)
                {
                    decimal? investmentPercentageGain = await service.CalculateInvestmentPercentageGain(walletId, shareValue.Value);

                    if (investmentPercentageGain.HasValue)
                    {
                        return Ok(investmentPercentageGain.Value);
                    }
                    else
                    {
                        return NotFound("Unable to calculate investment percentage gain.");
                    }
                }
                else
                {
                    return NotFound("Unable to retrieve share value.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetInvestmentPercentageChange: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
