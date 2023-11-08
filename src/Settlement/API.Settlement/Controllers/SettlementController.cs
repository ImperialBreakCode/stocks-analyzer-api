using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
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
		private readonly ITransactionMapperService _transactionMapperService;
		public SettlementController(ISettlementService settlementService, ITransactionMapperService transactionMapperService)
		{
			_settlementService = settlementService;
			_transactionMapperService = transactionMapperService;
		}
		[HttpPost]
		[Route("processTransactions")]
		public async Task<IActionResult> ProcessTransactions([FromBody] FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			try
			{
				var availabilityResponseDTO = await _settlementService.CheckAvailability(finalizeTransactionRequestDTO);
				var filteredAvailabilityResponseDTO = _transactionMapperService.FilterSuccessfulAvailabilityStockInfoDTOs(availabilityResponseDTO);

				_settlementService.ProcessTransaction(filteredAvailabilityResponseDTO);
				return StatusCode(200, availabilityResponseDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: An unexpected error occurred.");
			}

		}

		//да проверявам респонса който крис ми праща дали е успешен или не е и спрямо
		//това да си съхранявам транзакциите в базата (трябва да имам 2 таблици: успешни и неуспешни транзакции)
		//след това да правя нови опити да му изпращам неуспешните: 

	}
}