using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Analyzer.Domain.Interfaces;
using API.Accounts.Application.DTOs.Response;
using API.Analyzer.Infrastructure.Services;

namespace Analyzer.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAnalyzerUserService service;
        public UserController(IAnalyzerUserService service)
        {
            this.service = service;
        }

        [HttpGet("PortfolioSummary/{walletId}")]
        public async Task<IActionResult> GetPortfilioSummary(string walletId)
        {
            GetWalletResponseDTO jsonContent = await service.PortfolioSummary(walletId);
            if (jsonContent != null)
            {
                return Ok(jsonContent);
            }
            return StatusCode(500, "User profile not found");
        }
    

        [HttpGet("CurrentProfitability/{walletId}")]
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
    }
}

