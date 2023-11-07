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

        [HttpGet("get-action/{userId}")]
        public IActionResult GetAction(string userId)
        {
            return Ok();
        }

        [HttpGet("check-profitability/{userId}/{balance}")]
        public async Task<IActionResult> CheckProfitability(string userId, decimal balance)
        {
            decimal? result = await service.ProfitablenessAccountCheck(userId, balance);
            if (result.HasValue && result.Value >= balance)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpGet("get-info/{userId}")]
        public async Task<IActionResult> GetInfo(string userId)
        {
            Wallet jsonContent = await service.UserProfilInfo(userId);
            if (jsonContent != null)
            {
                return Ok(jsonContent);
            }
            return StatusCode(500, "User profile not found");
        }

        //[HttpGet("profit-change-per-day/{id}")]
        //public async Task<IActionResult> CalculateProfitChangePerDay(string userId)
        //{
        //    decimal? todayProfitability = await service.GetProfitabilityForDate(id, DateTime.Today);
        //    decimal? yesterdayProfitability = await service.GetProfitabilityForDate(id, DateTime.Today.AddDays(-1));
        //    if (todayProfitability.HasValue && yesterdayProfitability.HasValue && yesterdayProfitability.Value != 0)
        //    {
        //        decimal percentageChange = ((todayProfitability.Value - yesterdayProfitability.Value) / yesterdayProfitability.Value) * 100;

        //        return Ok(percentageChange);
        //    }
        //    else
        //    {
        //        return BadRequest("Unable to calculate percentage change");
        //    }
        //}
    } 
}
