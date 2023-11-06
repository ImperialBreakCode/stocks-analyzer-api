using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISellService
	{
		Task<FinalizeTransactionResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}