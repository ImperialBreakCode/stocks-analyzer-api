using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
	public interface IHangfireService
	{
		void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO);
	}
}