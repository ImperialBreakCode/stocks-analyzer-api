using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Settlement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementController : ControllerBase
	{
		private readonly ISettlementService _settlementService;
		public SettlementController(ISettlementService settlementService)
		{
			_settlementService = settlementService;
		}
		[HttpPost]
		[Route("processTransactions")]
		public IActionResult ProcessTransactions([FromBody] IEnumerable<FinalizeTransactionRequestDTO> requestStockDTOs)
		{
			_settlementService.ProcessTransactions(requestStockDTOs);

			return NoContent();
		}

	}
}