using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;

namespace API.Settlement.Domain.Interfaces.TransactionInterfaces.TransactionCompletionInterfaces
{
    public interface ITransactionCompletionService
    {
        Task FinalizeTransaction(AvailabilityResponseDTO availabilityResponseDTO);
    }
}
