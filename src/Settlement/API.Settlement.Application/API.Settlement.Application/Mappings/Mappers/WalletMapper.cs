﻿using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Mappings.Mappers
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
