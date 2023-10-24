using API.StockAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        StockServices _services = new();

        public StockController(StockServices services)
        {
            _services = services;
        }

        [HttpGet]
        [Route("Stock/Current/{symbol}")]
        public async Task<IActionResult> GetCurrentStock(string symbol)
        {
            if (symbol == null)
            {
                return BadRequest();
            }

            string function = "TIME_SERIES_INTRADAY";
            var result = await _services.GetCurrentStock(symbol, function);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
