using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface IAvailabilityResponseDTOMapper
    {
        AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO,
                                                            IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs);
        AvailabilityResponseDTO CreateCopyOfAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO);
        AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfoDTOs(AvailabilityResponseDTO availabilityResponseDTO);
    }
}
