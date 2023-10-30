using API.StockAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ExternalRequestService _externalRequestServices;
        private readonly StockService _stockServices;

        public StockController(StockService services, ExternalRequestService externalServices)
        {
            _stockServices = services;
            _externalRequestServices = externalServices;
        }

        [HttpGet]
        [Route("Current/{symbol}")]
        public async Task<IActionResult> GetCurrentStock(string symbol)
        {
            if (symbol == null)
            {
                return BadRequest();
            }

            string function = "TIME_SERIES_INTRADAY&interval=60min";
            var query = _externalRequestServices.QueryStringGenerator(symbol, function);
            var response = await _externalRequestServices.JsonDataGenerator(query);

            if (query == null)
            {
                return NotFound();
            }

            var result = await _stockServices.GetCurrentStock(response, symbol);
            return Ok(result);
        }

        [HttpGet]
        [Route("Daily/{symbol}")]
        public async Task<IActionResult> GetDailyStock(string symbol)
        {
            if (symbol == null)
            {
                return BadRequest();
            }

            string function = "TIME_SERIES_DAILY";
            var query = _externalRequestServices.QueryStringGenerator(symbol, function);
            var response = await _externalRequestServices.JsonDataGenerator(query);

            if (query == null)
            {
                return NotFound();
            }

            var result = await _stockServices.GetDailyStock(response, symbol);
            return Ok(result);
        }

        [HttpGet]
        [Route("Weekly/{symbol}")]
        public async Task<IActionResult> GetWeeklyStock(string symbol)
        {
            return Ok(symbol);
        }

        [HttpGet]
        [Route("Monthly/{symbol}")]
        public async Task<IActionResult> GetMonthlyStock(string symbol)
        {
            return Ok(symbol);
        }
    }
}
