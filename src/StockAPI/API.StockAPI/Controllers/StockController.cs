using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Infrastructure.Interfaces;
using API.StockAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IExternalRequestService _externalRequestServices;
        private readonly IStockService _stockServices;
        private readonly IContextServices _contextServices;
        private readonly ITimedOutCallServices _callServices;

        public StockController(IStockService services,
            IExternalRequestService externalServices,
            IContextServices contextServices,
            ITimedOutCallServices callServices)
        {
            _stockServices = services;
            _externalRequestServices = externalServices;
            _contextServices = contextServices;
            _callServices = callServices;
        }

        [HttpGet]
        [Route("{type}/{symbol}")]
        public async Task<IActionResult> GetStock(string symbol, string type)
        {
            if(string.IsNullOrEmpty(symbol) || string.IsNullOrEmpty(type))
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
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest();
                }

                var response = await _externalRequestServices.ExecuteQuery(symbol, query, type);
                if(response.StatusCode >= HttpStatusCode.InternalServerError)
                {
                    await _callServices.InsertFailedCallInDB(symbol, query, type);
                    return StatusCode(500);
                }

                var data = await _externalRequestServices.GetDataFromQuery(response);
                if (string.IsNullOrEmpty(data))
                {
                    return BadRequest();
                }

                var result = await _stockServices.GetStockFromResponse(symbol, data, type);
                if (result is null)
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
