using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface IHangfireJobService
	{
		Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO);
		Task RecurringFailedTransactionsJob();
		Task RecurringCapitalCheckJob();
		Task RecurringRabbitMQMessageSenderJob();
	}
}