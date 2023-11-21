using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces
{
	public interface ITransactionWrapper
	{
		IBuyService BuyService { get; }
		ISellService SellService { get; }
		Task<AvailabilityResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}