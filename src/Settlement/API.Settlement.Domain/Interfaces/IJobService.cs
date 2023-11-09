using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
	public interface IJobService
	{
		Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO);
	}
}