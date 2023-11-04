using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Settlement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementController : ControllerBase
	{
		private readonly ISettlementServiceWrapper _settlementServiceWrapper;
		public SettlementController(ISettlementServiceWrapper serviceWrapper)
		{
			_settlementServiceWrapper = serviceWrapper;
		}
		[HttpPost]
		[Route("processTransactions")]
		public async Task<IActionResult> ProcessTransactions([FromBody] IEnumerable<RequestStockDTO> requestStockDTOs)
		{
			var responseStockDTOs = await _settlementServiceWrapper.ProcessTransactions(requestStockDTOs);

			return NoContent();
		}

	}
}