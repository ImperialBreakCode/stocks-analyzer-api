using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

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
			var finalizeTransactionResponseDTO = await _settlementService.CheckAvailability(finalizeTransactionRequestDTO);
			_settlementService.ProcessTransaction(finalizeTransactionResponseDTO);
			return Ok(finalizeTransactionResponseDTO);
		}


	}
}