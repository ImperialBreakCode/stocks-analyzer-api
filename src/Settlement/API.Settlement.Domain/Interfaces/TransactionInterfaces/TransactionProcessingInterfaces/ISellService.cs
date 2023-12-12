using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces
{
    public interface ISellService
    {
        Task<AvailabilityResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
    }
}