using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Infrastructure.Interfaces;
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
        private readonly IContextServices _contextServices;

        public StockController(IStockService services, IExternalRequestService externalServices, IContextServices contextServices)
        {
            _stockServices = services;
            _externalRequestServices = externalServices;
            _contextServices = contextServices;
        }

        [HttpGet]
        [Route("{type}/{symbol}")]
        public async Task<IActionResult> GetStock(string symbol, string type)
        {
            if(symbol == "" || type == "")
            {
                return BadRequest();
            }

            var dbResult = await _contextServices.GetStockFromDB(symbol, type);

            if (dbResult is not null)
            {
                return Ok(dbResult);
            }

            try
            {
                var query = _externalRequestServices.QueryStringGenerator(symbol, type);
                if (query == null)
                {
                    return BadRequest();
                }

                var data = await _externalRequestServices.GetDataFromQuery(query);
                if (data == null)
                {
                    return BadRequest();
                }

                var result = await _stockServices.GetStockFromResponse(symbol, data, type);
                if (result == null)
                {
                    return NotFound();
                }

                await _contextServices.InsertStockInDB(result, type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
