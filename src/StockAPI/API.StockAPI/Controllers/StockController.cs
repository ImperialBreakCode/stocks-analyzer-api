using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IExternalRequestService _externalRequestServices;
        private readonly IStockService _stockServices;

        public StockController(IStockService services, IExternalRequestService externalServices)
        {
            _stockServices = services;
            _externalRequestServices = externalServices;
        }

        [HttpGet]
        [Route("{type}/{symbol}")]
        public async Task<IActionResult> GetStock(string symbol, string type)
        {
            if(symbol == "" || type == "")
            {
                return BadRequest();
            }

            try
            {
                var data = await _externalRequestServices.GetData(symbol, type);
                var result = await _stockServices.GetStockFromRequest(symbol, data, type);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
