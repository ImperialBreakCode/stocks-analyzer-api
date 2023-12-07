using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
	public interface IFinalizeTransactionResponseDTOMapper
    {
        FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(AvailabilityResponseDTO availabilityResponseDTO);
        FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(Transaction transaction);
		IEnumerable<FinalizeTransactionResponseDTO> MapToFinalizeTransactionResponseDTOs(IEnumerable<Transaction> walletAndIsSaleTransactions);

    }
}
