using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementService : ISettlementService
	{
		private readonly ITransactionWrapper _transactionWrapper;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHangfireService _hangfireService;

		public SettlementService(ITransactionWrapper transactionWrapper,
								IMapperManagementWrapper transactionMapperService,
								IHangfireService hangfireService)
		{
			_transactionWrapper = transactionWrapper;
			_hangfireService = hangfireService;
			_mapperManagementWrapper = transactionMapperService;
		}
		public async Task<AvailabilityResponseDTO> ProcessTransactions(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await _transactionWrapper.CheckAvailability(finalizeTransactionRequestDTO);
			var clonedAvailabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.CloneAvailabilityResponseDTO(availabilityResponseDTO);
			var filteredAvailabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.FilterSuccessfulAvailabilityStockInfoDTOs(clonedAvailabilityResponseDTO);

			_hangfireService.ScheduleStockProcessingJob(filteredAvailabilityResponseDTO);
			_hangfireService.InitializeRecurringFailedTransactionsJob();
			_hangfireService.InitializeRecurringCapitalLossJobCheck();
			_hangfireService.InitializeRecurringRabbitMQMessageSenderJob();

			return availabilityResponseDTO;
		}

	}
}