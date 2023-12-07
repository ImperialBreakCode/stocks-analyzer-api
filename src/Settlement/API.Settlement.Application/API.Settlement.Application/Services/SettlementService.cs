using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.HangfireInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces;

namespace API.Settlement.Application.Services
{
	public class SettlementService : ISettlementService
	{
		private readonly ITransactionProcessingService _transactionWrapper;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHangfireService _hangfireService;

		public SettlementService(ITransactionProcessingService transactionWrapper,
								 IMapperManagementWrapper mapperManagementWrapper,
								 IHangfireService hangfireService)
		{
			_transactionWrapper = transactionWrapper;
			_hangfireService = hangfireService;
			_mapperManagementWrapper = mapperManagementWrapper;
		}
		public async Task<AvailabilityResponseDTO> ProcessTransaction(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await ProcessTransactionRequests(finalizeTransactionRequestDTO);

			var copiedAvailabilityResponseDTO = CreateCopyOfAvailabilityResponseDTO(availabilityResponseDTO);
			var filteredAvailabilityResponseDTO = FilterSuccessfulAvailabilityStockInfo(copiedAvailabilityResponseDTO);

			ScheduleStockProcessingJob(filteredAvailabilityResponseDTO);
			InitializeRecurringJobs();

			return availabilityResponseDTO;
		}

		private async Task<AvailabilityResponseDTO> ProcessTransactionRequests(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			return await _transactionWrapper.ProcessTransactions(finalizeTransactionRequestDTO);
		}

		private AvailabilityResponseDTO CreateCopyOfAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.CreateCopyOfAvailabilityResponseDTO(availabilityResponseDTO);
		}
		private AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfo(AvailabilityResponseDTO availabilityResponseDTO)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.FilterSuccessfulAvailabilityStockInfoDTOs(availabilityResponseDTO);
		}

		private void ScheduleStockProcessingJob(AvailabilityResponseDTO filteredSuccessfulAvailabilityResponseDTO)
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