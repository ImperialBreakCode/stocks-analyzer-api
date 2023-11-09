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

		//да проверявам респонса който крис ми праща дали е успешен или не е и спрямо
		//това да си съхранявам транзакциите в базата (трябва да имам 2 таблици: успешни и неуспешни транзакции)
		//след това да правя нови опити да му изпращам неуспешните: 
	}
}