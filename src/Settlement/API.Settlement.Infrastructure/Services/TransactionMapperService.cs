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
		public ResponseStockDTO CreateTransactionResponse(RequestStockDTO requestStockDTO, decimal totalPriceIncludingCommission, Status status)
		{
			var responseStockDTO = _mapper.Map<ResponseStockDTO>(requestStockDTO);
			responseStockDTO.IsSuccessful = status == Status.Scheduled;
			responseStockDTO.Message = GetMessageBasedOnStatus(status);
			responseStockDTO.SinglePrice = GetSinglePriceWithCommission(totalPriceIncludingCommission, responseStockDTO.Quantity);
			responseStockDTO.TotalPriceIncludingCommission = totalPriceIncludingCommission;
			return responseStockDTO;
		}
		public Wallet CreateStockDTO(RequestStockDTO requestStockDTO)
		{
			return _mapper.Map<Wallet>(requestStockDTO);
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