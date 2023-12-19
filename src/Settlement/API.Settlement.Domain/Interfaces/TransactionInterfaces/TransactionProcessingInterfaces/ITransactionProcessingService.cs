using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces
{
    public interface ITransactionProcessingService
    {
        IBuyService BuyService { get; }
        ISellService SellService { get; }
        Task<AvailabilityResponseDTO> ProcessTransactions(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
    }
}