using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.StockService;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost]
        [Route("AddStockForPurchase")]
        public async Task<IActionResult> AddStockForPurchase(StockActionDTO stockAction)
        {
            string response = await _stockService.AddForPurchase(stockAction);

            if (response == ResponseMessages.WalletNotFound)
            {
                return NotFound(response);
            }
            else if (response == String.Format(ResponseMessages.StockActionSuccessfull, "purchase"))
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        [Route("AddStockForSale")]
        public IActionResult AddStockForSale(StockActionDTO stockActionDTO)
        {
            string response = _stockService.AddForSale(stockActionDTO);

            if (response == String.Format(ResponseMessages.StockActionSuccessfull, "sale"))
            {
                return Ok(response);
            }
            else if (response == ResponseMessages.StockNotEnoughStocksToSale)
            {
                return BadRequest(response);
            }

            return NotFound(response);
        }

        [HttpPost]
        [Route("ConfirmPurchase/{walletId}")]
        public IActionResult ConfirmPurchase(string walletId)
        {
            return Ok();
        }

        [HttpPost]
        [Route("ConfirmSale/{walletId}")]
        public IActionResult ConfirmSale(string walletId)
        {
            return Ok();
        }
    }
}
