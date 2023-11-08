using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISettlementService
	{
		void ProcessTransaction(AvailabilityResponseDTO availabilityResponseDTO);

		Task<AvailabilityResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}