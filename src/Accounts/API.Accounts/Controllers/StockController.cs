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
        [Route("AddStockForPurchase")]
        public async Task<IActionResult> AddStockForPurchase(StockActionDTO stockAction)
        {
            string response = await _stockService.ActionManager.AddForPurchase(stockAction);

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

        [HttpPut]
        [Route("AddStockForSale")]
        public IActionResult AddStockForSale(StockActionDTO stockActionDTO)
        {
            string response = _stockService.ActionManager.AddForSale(stockActionDTO);

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
        public async Task<IActionResult> ConfirmPurchase(string walletId)
        {
            string response = await _stockService.ActionFinalizer.ConfirmPurchase(walletId);

            if (response == ResponseMessages.WalletNotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("ConfirmSale/{walletId}")]
        public async Task<IActionResult> ConfirmSale(string walletId)
        {
            string response = await _stockService.ActionFinalizer.ConfirmSales(walletId);

            if (response == ResponseMessages.WalletNotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
