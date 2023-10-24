using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Analyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IService _service;
        public AccountController(IService service)
        {
            _service = service;
        }

        public bool CheckProfitability(int Id, decimal amount)
        {
            return _service.ProfitablenessAccountCheck(Id, amount);
        }
    }
}
