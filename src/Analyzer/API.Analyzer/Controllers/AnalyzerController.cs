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


        //[HttpGet("check-profitability/{username")]
        //public async Task<IActionResult> CheckProfitability(string userName,decimal balance)
        //{
        //    decimal? result = await service.ProfitablenessAccountCheck(userName);
        //    if (result.HasValue && result.Value >= balance)
        //    {
        //        return Ok(true);
        //    }
        //    return BadRequest(false);
        //}

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

        //[HttpGet("profit-change-per-day/{id}")]
        //public async Task<IActionResult> CalculateProfitChangePerDay(string userName)
        //{
        //    decimal? todayprofitability = await service.GetProfitabilityForDate(userName, DateTime.Today);
        //    decimal? yesterdayprofitability = await service.GetProfitabilityForDate(userName, DateTime.Today.AddDays(-1));
        //    if (todayprofitability.HasValue && yesterdayprofitability.HasValue && yesterdayprofitability.Value != 0)
        //    {
        //        decimal percentagechange = ((todayprofitability.Value - yesterdayprofitability.Value) / yesterdayprofitability.Value) * 100;

        //        return Ok(percentagechange);
        //    }
        //    else
        //    {
        //        return BadRequest("unable to calculate percentage change");
        //    }
        //}
    } 
}
