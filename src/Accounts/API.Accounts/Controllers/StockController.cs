using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.StockService;
using API.Accounts.Extensions;
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

        [HttpGet]
        [Route("GetStock/{stockId}")]
        public IActionResult GetStock(string stockId)
        {
            GetStockResponseDTO? resposne = _stockService.GetStockById(stockId);

            if (resposne is null)
            {
                return NotFound();
            }

            return Ok(resposne);
        }

        [HttpGet]
        [Route("GetStocksInWallet/{walletId}")]
        public IActionResult GetStocksInWallet(string walletId)
        {
            ICollection<GetStockResponseDTO>? resposne = _stockService.GetStocksByWalletId(walletId);

            if (resposne is null)
            {
                return NotFound("Wallet not found");
            }

            return Ok(resposne);
        }

        [HttpPut]
        [Route("AddStockForPurchase/{username}")]
        public async Task<IActionResult> AddStockForPurchase([FromBody] StockActionDTO stockAction, [FromRoute] string username)
        {
            string response = await _stockService.ActionManager.AddForPurchase(stockAction, username);

            return this.ParseAndReturnMessage(response);
        }

        [HttpPut]
        [Route("AddStockForSale/{username}")]
        public async Task<IActionResult> AddStockForSale([FromBody] StockActionDTO stockActionDTO, [FromRoute] string username)
        {
            string response = await _stockService.ActionManager.AddForSale(stockActionDTO, username);

            return this.ParseAndReturnMessage(response);
        }

        [HttpPost]
        [Route("ConfirmPurchase/{username}")]
        public async Task<IActionResult> ConfirmPurchase(string username)
        {
            string response = await _stockService.ActionFinalizer.ConfirmPurchase(username);

            return this.ParseAndReturnMessage(response);
        }

        [HttpPost]
        [Route("ConfirmSale/{username}")]
        public async Task<IActionResult> ConfirmSale(string username)
        {
            string response = await _stockService.ActionFinalizer.ConfirmSales(username);
            
            return this.ParseAndReturnMessage(response);
        }
    }
}
