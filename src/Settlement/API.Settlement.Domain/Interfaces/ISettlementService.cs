using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
	public interface ISettlementService
	{
		Task<AvailabilityResponseDTO> ProcessTransaction(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}