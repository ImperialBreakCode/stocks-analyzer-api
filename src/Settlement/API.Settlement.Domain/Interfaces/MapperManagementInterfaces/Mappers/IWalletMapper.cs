using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface IWalletMapper
    {
        Wallet MapToWalletEntity(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);

    }
}
