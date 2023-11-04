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

        [HttpPost]
        [Route("CreateWallet/{username}")]
        public IActionResult CreateWallet(string username)
        {
            string response = _walletService.CreateWallet(username);

            if (response == ResponseMessages.WalletCreated)
            {
                return StatusCode(StatusCodes.Status201Created, response);
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
        public IActionResult GetWallet(string id)
        {
            var response = _walletService.GetWallet(id);

            if (response is null)
            {
                return NotFound("wallet is not found");
            }

            return Ok(response);
        }
    }
}
