using API.Analyzer.Domain.DTOs;
using API.Analyzer.Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Analyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerController : ControllerBase
    {
        private readonly IService service;
        public AnalyzerController(IService service)
        {
            this.service = service;
        }

        [HttpGet("get-action/{userId}")]
        public IActionResult GetAction(int userId)
        {
            return Ok();
        }
        [HttpGet("get-user/{amount}")]
        public bool CheckProfitability(int id, decimal amount)
        {
            return service.ProfitablenessAccountCheck(id, amount);
        }

        [HttpGet("get-info/{id}")]
        public async Task<IActionResult> GetInfo(int id)
        {
            User jsonContent = await service.UserProfilInfo(id);
            if (jsonContent != null)
            {
                return Ok(jsonContent);
            }
            return StatusCode(500, "Ami greshkaaa");
        }

        //public void Action(string userId)
        //{
        //    _service.UserPortfolioProfit(userId);
        //}
    }
}
