using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.WalletService;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPut]
        public IActionResult Deposit()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult CloseWallet()
        {
            return Ok();
        }

        [HttpPost]
        [Route("CreateWallet/{username}")]
        public IActionResult CreateWallet(string username)
        {
            string response = _walletService.CreateWallet(username);

            if (response == ResponseMessages.WalletCreated)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }
            else if (response == ResponseMessages.WalletAlreadyExists)
            {
                return BadRequest(response);
            }

            return NotFound(response);
        }

        [HttpGet]
        [Route("GetWalletBalance/{walletId}")]
        public IActionResult GetWalletBalance(string walletId)
        {
            var response = _walletService.GetWallet(walletId);

            if (response is null)
            {
                return NotFound("wallet is not found");
            }

            return Ok(response.Balance);
        }

        [HttpGet]
        [Route("GetWallet/{walletId}")]
        public IActionResult GetWallet(string walletId)
        {
            var response = _walletService.GetWallet(walletId);

            if (response is null)
            {
                return NotFound("wallet is not found");
            }

            return Ok(response);
        }
    }
}
