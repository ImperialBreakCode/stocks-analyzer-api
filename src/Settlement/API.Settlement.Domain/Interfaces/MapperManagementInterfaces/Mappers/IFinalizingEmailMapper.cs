using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
	public interface IFinalizingEmailMapper
    {
        FinalizingEmail CreateTransactionSummaryEmailDTO(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, string subject);
    }
}
