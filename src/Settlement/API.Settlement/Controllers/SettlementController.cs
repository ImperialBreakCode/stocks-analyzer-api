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
		public async Task<IActionResult> ProcessTransactions([FromBody] FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await _settlementService.ProcessTransactions(finalizeTransactionRequestDTO);

			return StatusCode(200, availabilityResponseDTO);
		}

	}
}