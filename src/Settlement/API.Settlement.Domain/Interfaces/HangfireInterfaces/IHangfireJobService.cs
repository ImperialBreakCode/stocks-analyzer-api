using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces.HangfireInterfaces
{
	public interface IHangfireJobService
	{
		Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO);
		Task RecurringFailedTransactionsJob();
		Task RecurringCapitalCheckJob();
		void RecurringRabbitMQMessageSenderJob();
	}
}