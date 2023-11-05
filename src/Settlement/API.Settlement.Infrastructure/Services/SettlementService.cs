using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementService : ISettlementService
	{
		private readonly IHangfireService _hangfireService;

		public SettlementService(IHangfireService hangfireService)
		{
			_hangfireService = hangfireService;
		}
		public void ProcessTransactions(IEnumerable<FinalizeTransactionRequestDTO> finalizeTransactionRequestDTOs)
		{
			_hangfireService.ScheduleStockProcessingJob(finalizeTransactionRequestDTOs);
		}

	}
}