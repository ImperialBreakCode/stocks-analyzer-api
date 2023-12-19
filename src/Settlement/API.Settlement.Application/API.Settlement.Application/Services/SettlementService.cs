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
		private readonly ITransactionProcessingService _transactionProcessingService;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHangfireService _hangfireService;

		public SettlementService(ITransactionProcessingService transactionProcessingService,
								 IMapperManagementWrapper mapperManagementWrapper,
								 IHangfireService hangfireService)
		{
			_transactionProcessingService = transactionProcessingService;
			_hangfireService = hangfireService;
			_mapperManagementWrapper = mapperManagementWrapper;
		}
		public async Task<AvailabilityResponseDTO> ProcessTransaction(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityResponseDTO = await _transactionProcessingService.ProcessTransactions(finalizeTransactionRequestDTO);

			var copiedAvailabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.CreateCopyOfAvailabilityResponseDTO(availabilityResponseDTO);
			var filteredSuccessfulAvailabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.FilterSuccessfulAvailabilityStockInfoDTOs(copiedAvailabilityResponseDTO);

			_hangfireService.ScheduleStockProcessingJob(filteredSuccessfulAvailabilityResponseDTO);
			_hangfireService.InitializeRecurringFailedTransactionsJob();
			_hangfireService.InitializeRecurringCapitalLossJobCheck();
			_hangfireService.InitializeRecurringRabbitMQMessageSenderJob();

			return availabilityResponseDTO;
		}

	}
}