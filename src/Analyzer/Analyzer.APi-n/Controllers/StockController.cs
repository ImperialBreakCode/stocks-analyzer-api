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
            catch (HttpRequestException)
            {
                return StatusCode(500, "Internal Server Error: Unable to retrieve data from external services.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("PercentageChange/{username}/{symbol}/{type}")]
        public async Task<ActionResult<decimal?>> GetInvestmentPercentageChange(string username,string symbol,string type)
        {
            try
            {
                decimal? shareValue = await service.GetShareValue(username);

                if (shareValue.HasValue)
                {
                    decimal? investmentPercentageGain = await service.CalculateInvestmentPercentageGain(symbol,type, shareValue.Value);

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
            catch (HttpRequestException)
            {
                return StatusCode(500, "Internal Server Error: Unable to retrieve data from external services.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
