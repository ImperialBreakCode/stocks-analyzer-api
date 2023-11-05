using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface ITransactionMapperService
	{
		StockInfoResponseDTO MapToStockResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status);
		FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<StockInfoResponseDTO> stockInfoResponseDTOs);
	}
}