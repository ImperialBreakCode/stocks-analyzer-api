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
        //maybe make this route have "adjusted" or "averaged" or something in front of it
        [HttpGet]
        [Route("{type}/{symbol}")]
        public async Task<IActionResult> GetStock(string symbol, string type)
        {
            if(string.IsNullOrEmpty(symbol) || string.IsNullOrEmpty(type))
            {
                return BadRequest("Invalid input.");
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
                    return NoContent();
                }

                var response = await _externalRequestServices.ExecuteQuery(symbol, query, type);

                if (response is null)
                {
                    return NoContent();
                }

                var data = await _externalRequestServices.GetDataFromQuery(response);
                if (!_externalRequestServices.CheckIfDataIsValid(response, data))
                {
                    await _callServices.InsertFailedCallInDB(symbol, query, type);
                    return StatusCode(500, "Connection to external resources failed , the request has been logged.");
                }

                var result = await _stockServices.GetStockFromResponse(symbol, data, type);
                if (result is null)
                {
                    return NoContent();
                }

                _contextServices.InsertStockInDB(result, type);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode(500, "An internal server error has ocurred: " + exception.Message);
            }
        }

        // if i do the above change, probably change the name of this route from "stocks" to "raw"
        [HttpGet]
        [Route("stocks/{type}/{symbol}")]
        public async Task<IActionResult> GetStockList(string symbol, string type)
        {
            if (symbol == "" || type == "")
            {
                return BadRequest();
            }

            try
            {
                var query = _externalRequestServices.QueryStringGenerator(symbol, type);
                if (string.IsNullOrEmpty(query))
                {
                    return NoContent();
                }

                var response = await _externalRequestServices.ExecuteQuery(symbol, query, type);

                var data = await _externalRequestServices.GetDataFromQuery(response);
                if (!_externalRequestServices.CheckIfDataIsValid(response, data))
                {
                    await _callServices.InsertFailedCallInDB(symbol, query, type);
                    return StatusCode(500, "Connection to external resources failed , the request has been logged.");
                }

                var result = await _stockServices.GetStockList(symbol, data, type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error has ocurred: " + ex.Message);
            }
        }
    }
}
