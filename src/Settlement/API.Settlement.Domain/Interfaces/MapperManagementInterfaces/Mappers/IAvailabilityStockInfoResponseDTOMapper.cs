using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Infrastructure.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface IAvailabilityStockInfoResponseDTOMapper
    {
        AvailabilityStockInfoResponseDTO MapToAvailabilityStockInfoResponseDTO(StockInfoRequestDTO stockInfoRequestDTO,
                                                                                decimal totalPriceIncludingCommission,
                                                                                Status status);
    }
}
