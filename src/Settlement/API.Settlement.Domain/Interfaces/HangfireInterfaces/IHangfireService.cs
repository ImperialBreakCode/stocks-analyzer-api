using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces.HangfireInterfaces
{
	public interface IHangfireService
	{
		void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO);
		void InitializeRecurringFailedTransactionsJob();
		void InitializeRecurringCapitalLossJobCheck();
		void InitializeRecurringRabbitMQMessageSenderJob();
	}
}