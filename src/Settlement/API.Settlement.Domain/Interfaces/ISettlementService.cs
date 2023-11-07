using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISettlementService
	{
		void ProcessTransaction(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);

		Task<FinalizeTransactionResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}