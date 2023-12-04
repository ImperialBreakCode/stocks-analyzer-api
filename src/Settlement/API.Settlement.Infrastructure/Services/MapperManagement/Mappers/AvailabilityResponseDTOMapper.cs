using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class AvailabilityResponseDTOMapper : IAvailabilityResponseDTOMapper
    {
        private readonly IMapper _mapper;
        public AvailabilityResponseDTOMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
        {
            var availabilityResponseDTO = _mapper.Map<AvailabilityResponseDTO>(finalizeTransactionRequestDTO);
            availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = availabilityStockInfoResponseDTOs;

            return availabilityResponseDTO;
        }
        public AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfoDTOs(AvailabilityResponseDTO availabilityResponseDTO)
        {
            var successfulAvailabilityStockInfoDTOs = availabilityResponseDTO.AvailabilityStockInfoResponseDTOs.Where(x => x.IsSuccessful).ToList();

            availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = successfulAvailabilityStockInfoDTOs;

            return availabilityResponseDTO;
        }
        public AvailabilityResponseDTO CloneAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
        {
            return _mapper.Map<AvailabilityResponseDTO>(availabilityResponseDTO);
        }
    }
}
