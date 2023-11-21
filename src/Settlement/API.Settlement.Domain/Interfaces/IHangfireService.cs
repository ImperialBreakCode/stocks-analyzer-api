using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface IHangfireService
	{
		void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO, UserType userRank);
		void InitializeRecurringFailedTransactionsJob();
		void InitializeRecurringCapitalLossJobCheck();
	}
}