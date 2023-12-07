﻿using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.CommissionInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;

namespace API.Settlement.Application.Mappings.Mappers
{
	public class AvailabilityStockInfoResponseDTOMapper : IAvailabilityStockInfoResponseDTOMapper
	{
		private readonly IMapper _mapper;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IUserCommissionService _commissionService;

		public AvailabilityStockInfoResponseDTOMapper(IMapper mapper,
													  IInfrastructureConstants infrastructureConstants,
													  IUserCommissionService commissionService)
		{
			_mapper = mapper;
			_infrastructureConstants = infrastructureConstants;
			_commissionService = commissionService;
		}

		public AvailabilityStockInfoResponseDTO MapToAvailabilityStockInfoResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status)
		{
			var availabilityStockResponseDTO = _mapper.Map<AvailabilityStockInfoResponseDTO>(stockInfoRequestDTO);
			availabilityStockResponseDTO.IsSuccessful = status == Status.Scheduled;
			availabilityStockResponseDTO.Message = _infrastructureConstants.GetMessageBasedOnStatus(status);
			availabilityStockResponseDTO.SinglePriceIncludingCommission = _commissionService.CalculateSinglePriceWithCommission(totalPriceIncludingCommission, availabilityStockResponseDTO.Quantity);

			return availabilityStockResponseDTO;
		}
	}
}
