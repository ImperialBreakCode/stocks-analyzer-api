using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface ITransactionMapperService
	{
		BuyStockResponseDTO CreateBuyTransactionResponse(BuyStockDTO buyStockDTO, decimal totalBuyingPriceIncludingCommission, Status status);
		SellStockResponseDTO CreateSellTransactionResponse(SellStockDTO sellStockDTO, decimal totalSellingPriceIncludingCommission);
	}
}