using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Analyzer.Domain.Interfaces;
using API.Analyzer.Domain.DTOs;

namespace Analyzer.APi_n.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IApiService service;
        public TestController(IApiService service)
        {
            this.service = service;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        //[HttpGet("get-info/{userName}")]
        //public async Task<IActionResult> GetInfo(string userName)
        //{
        //    GetWalletResponseDTO jsonContent = await service.UserProfilInfo(userName);
        //    if (jsonContent != null)
        //    {
        //        return Ok(jsonContent);
        //    }
        //    return StatusCode(500, "User profile not found");
        //}

        [HttpGet("currentProfitability/{walletId}")]
        public async Task<ActionResult<decimal>> GetCurrentProfitability(string walletId)
        {
            try
            {
                decimal profitability = await service.CurrentProfitability(walletId);

                if (profitability >= 0)
                {
                    return Ok(profitability);
                }

                Console.WriteLine($"Error retrieving profitability for walletId {walletId}");
                return NotFound($"Profitability not available for walletId {walletId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving profitability for walletId {walletId}: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
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

        //[HttpGet("portfolio-risk")]

        //[HttpGet("daily-profitability-changes")]
    }
}

