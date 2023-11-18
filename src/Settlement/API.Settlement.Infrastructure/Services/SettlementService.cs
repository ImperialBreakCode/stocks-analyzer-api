using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementService : ISettlementService
	{
		private readonly ITransactionWrapper _transactionWrapper;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IHangfireService _hangfireService;

		public SettlementService(ITransactionWrapper transactionWrapper,
								ITransactionMapperService transactionMapperService,
								IHangfireService hangfireService)
		{
			_transactionWrapper = transactionWrapper;
			_hangfireService = hangfireService;
			_transactionMapperService = transactionMapperService;
		}
		public async Task<AvailabilityResponseDTO> ProcessTransactions(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await _transactionWrapper.CheckAvailability(finalizeTransactionRequestDTO);
			var clonedAvailabilityResponseDTO = _transactionMapperService.CloneAvailabilityResponseDTO(availabilityResponseDTO);
			var filteredAvailabilityResponseDTO = _transactionMapperService.FilterSuccessfulAvailabilityStockInfoDTOs(clonedAvailabilityResponseDTO);

			_hangfireService.ScheduleStockProcessingJob(filteredAvailabilityResponseDTO);
			_hangfireService.InitializeRecurringFailedTransactionsJob();

			return availabilityResponseDTO;
		}

	}
}