using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

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
		public IActionResult ProcessTransactions([FromBody] FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			_settlementService.ProcessTransaction(finalizeTransactionRequestDTO);
            return NoContent();
		}

		
	}
}