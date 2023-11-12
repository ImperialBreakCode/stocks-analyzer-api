using API.Accounts.Application.DTOs;
using API.Accounts.Application.DTOs.Enums;
using API.Accounts.Application.DTOs.Request;
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
        [Route("Deposit")]
        public IActionResult Deposit(DepositWalletDTO depositDTO)
        {
            string response = _walletService.Deposit(depositDTO);

            if (response == ResponseMessages.WalletNotFound)
            {
                return NotFound(response);
            }

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
