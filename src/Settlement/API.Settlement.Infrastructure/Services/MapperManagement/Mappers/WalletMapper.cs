using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class WalletMapper : IWalletMapper
    {
        private readonly IMapper _mapper;

        public WalletMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Wallet MapToWalletEntity(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
        {
            return _mapper.Map<Wallet>(finalizeTransactionResponseDTO);
        }

    }
}
