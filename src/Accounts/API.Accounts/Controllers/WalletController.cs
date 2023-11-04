using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        [HttpPut]
        public IActionResult Deposit()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetWalletBalance(string id)
        {
            return Ok();
        }
    }
}
