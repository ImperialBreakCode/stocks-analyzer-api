using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using AutoMapper;

namespace API.Settlement.Infrastructure.Services
{
	public class TransactionMapperService : ITransactionMapperService
	{
		private readonly IMapper _mapper;
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public TransactionMapperService(IMapper mapper, IInfrastructureConstants constants)
		{
			_mapper = mapper;
			_InfrastructureConstants = constants;
		}
		public StockInfoResponseDTO MapToStockResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status)
		{
			var stockInfoResponseDTO = _mapper.Map<StockInfoResponseDTO>(stockInfoRequestDTO);
			stockInfoResponseDTO.IsSuccessful = status == Status.Success;
			stockInfoResponseDTO.Message = GetMessageBasedOnStatus(status);
			stockInfoResponseDTO.SinglePriceIncludingCommission = GetSinglePriceWithCommission(totalPriceIncludingCommission, stockInfoResponseDTO.Quantity);

			return stockInfoResponseDTO;
		}

		public FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<StockInfoResponseDTO> stockInfoResponseDTOs)
		{
			var finalizeTransactionResponseDTO = _mapper.Map<FinalizeTransactionResponseDTO>(finalizeTransactionRequestDTO);
			finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

			return finalizeTransactionResponseDTO;
		}
		private decimal GetSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity)
		{
			return totalPriceIncludingCommission / quantity;
		}

		private string GetMessageBasedOnStatus(Status status)
		{
			switch (status)
			{
				case Status.Declined: return _InfrastructureConstants.TransactionDeclinedMessage;
				case Status.Success: return _InfrastructureConstants.TransactionSuccessMessage;
				case Status.Scheduled: return _InfrastructureConstants.TransactionScheduledMessage;
				default: return String.Empty;

			}
		}

		
	}
}