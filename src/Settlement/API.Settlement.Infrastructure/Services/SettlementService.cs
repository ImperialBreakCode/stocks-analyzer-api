using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementService : ISettlementService
	{
		private readonly IHangfireService _hangfireService;
		private readonly ITransactionWrapper _transactionWrapper;

		public SettlementService(IHangfireService hangfireService,
								ITransactionWrapper transactionWrapper)
		{
			_hangfireService = hangfireService;
			_transactionWrapper = transactionWrapper;
		}


		public async Task<AvailabilityResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			return await _transactionWrapper.CheckAvailability(finalizeTransactionRequestDTO);
		}
		public void ProcessTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			_hangfireService.ScheduleStockProcessingJob(availabilityResponseDTO);
		}

	}
}