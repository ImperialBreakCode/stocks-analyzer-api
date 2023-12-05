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
								 IMapperManagementWrapper mapperManagementWrapper, 
								 IHangfireService hangfireService)
		{
			_transactionWrapper = transactionWrapper; 
			_hangfireService = hangfireService; 
			_mapperManagementWrapper = mapperManagementWrapper;
		}
		public async Task<AvailabilityResponseDTO> ProcessTransactions(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await CheckWalletAvailability(finalizeTransactionRequestDTO);

			var filteredSuccessfulAvailabilityResponseDTO = FilterSuccessfulAvailabilityStockInfo(availabilityResponseDTO);

			ScheduleStockProcessingJobs(filteredSuccessfulAvailabilityResponseDTO);
			InitializeRecurringJobs();

			return availabilityResponseDTO;
		}

		private async Task<AvailabilityResponseDTO> CheckWalletAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			return await _transactionWrapper.CheckWalletAvailability(finalizeTransactionRequestDTO);
		}

		private AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfo(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var clonedAvailabilityResponseDTO = CreateCopyOfAvailabilityResponseDTO(availabilityResponseDTO);
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.FilterSuccessfulAvailabilityStockInfoDTOs(clonedAvailabilityResponseDTO);
		}

		private AvailabilityResponseDTO CreateCopyOfAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.CreateCopyOfAvailabilityResponseDTO(availabilityResponseDTO);
		}
		private void ScheduleStockProcessingJobs(AvailabilityResponseDTO filteredSuccessfulAvailabilityResponseDTO)
		{
			_hangfireService.ScheduleStockProcessingJob(filteredSuccessfulAvailabilityResponseDTO);
		}
		private void InitializeRecurringJobs()
		{
			_hangfireService.InitializeRecurringFailedTransactionsJob();
			_hangfireService.InitializeRecurringCapitalLossJobCheck();
			_hangfireService.InitializeRecurringRabbitMQMessageSenderJob();
		}

	}
}