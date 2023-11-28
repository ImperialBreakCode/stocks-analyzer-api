using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
	public interface IBuyService
	{
		Task<AvailabilityResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}