using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
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


		public void ProcessTransaction(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			_hangfireService.ScheduleStockProcessingJob(finalizeTransactionResponseDTO);
		}
		public async Task<FinalizeTransactionResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			return await _transactionWrapper.CheckAvailability(finalizeTransactionRequestDTO);
		}

	}
}