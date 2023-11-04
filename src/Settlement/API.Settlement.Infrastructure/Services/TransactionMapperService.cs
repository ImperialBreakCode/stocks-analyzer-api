using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
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

		public BuyStockResponseDTO CreateBuyTransactionResponse(BuyStockDTO buyStockDTO, decimal totalBuyingPriceIncludingCommission, Status status)
		{
			var buyStockResponseDTO = _mapper.Map<BuyStockResponseDTO>(buyStockDTO);
			buyStockResponseDTO.IsSuccessful = status == Status.Scheduled;
			buyStockResponseDTO.Message = GetMessageBasedOnStatus(status);
			buyStockResponseDTO.TotalBuyingPriceIncludingCommission = totalBuyingPriceIncludingCommission;
			return buyStockResponseDTO;
		}

		public SellStockResponseDTO CreateSellTransactionResponse(SellStockDTO sellStockDTO, decimal totalSellingPriceIncludingCommission)
		{
			var sellStockResponseDTO = _mapper.Map<SellStockResponseDTO>(sellStockDTO);
			sellStockResponseDTO.IsSuccessful = true;
			sellStockResponseDTO.Message = _InfrastructureConstants.TransactionScheduledMessage;
			sellStockResponseDTO.TotalSellingPriceIncludingCommission = totalSellingPriceIncludingCommission;
			return sellStockResponseDTO;
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