using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.Services.TransactionService;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("CompleteTransaction")]
        public IActionResult CompleteTransactions(FinalizeTransactionDTO transactionInfo)
        {
            if (_transactionService.CompleteTransactions(transactionInfo))
            {
                return Ok();
            }

            return NotFound("Wallet not found");
        }
    }
}
