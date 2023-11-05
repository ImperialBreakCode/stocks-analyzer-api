using API.Accounts.Application.DTOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        [Route("CompleteTransaction")]
        public IActionResult CompleteTransactions(ICollection<TransactionInfoDTO> transactionInfo)
        {
            return Ok();
        }
    }
}
