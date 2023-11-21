using API.Analyzer.Domain.DTOs;
using API.Accounts.Domain.Entities;
using API.Analyzer.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Analyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerController : ControllerBase
    {
        private readonly IApiService service;
        public AnalyzerController(IApiService service)
        {
            this.service = service;
        }

        [HttpGet("get-info/{userName}")]
        public async Task<IActionResult> GetInfo(string userName)
        {
            GetWalletResponseDTO jsonContent = await service.UserProfilInfo(userName);
            if (jsonContent != null)
            {
                return Ok(jsonContent);
            }
            return StatusCode(500, "User profile not found");
        }

        [HttpGet("check-profitability/{userName}")]
        public async Task<IActionResult> GetCurrentProfitability(string userName, decimal? balance)
        {
            decimal? result = await service.CurrentProfitability(userName);
            if (result.HasValue && result.Value >= balance)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("percentage-change/{symbol}")]
        public async Task<IActionResult> PercentageChange(string symbol)
        {
            try
            {
                decimal percentageChange = await service.PercentageChange(symbol);
                return Ok(percentageChange);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
