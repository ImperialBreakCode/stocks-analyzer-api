using API.Settlement.Domain.DTOs;
using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IBuyService
	{
		Task<FinalizeTransactionResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}