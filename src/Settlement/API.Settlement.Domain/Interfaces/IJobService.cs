using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface IJobService
	{
		Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO, UserType userRank);
		Task RecurringFailedTransactionsJob();
		Task RecurringCapitalLossCheckJob();
	}
}