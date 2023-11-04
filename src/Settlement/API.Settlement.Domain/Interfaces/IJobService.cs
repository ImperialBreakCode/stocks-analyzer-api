using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IJobService
	{
		void ProcessNextDayAccountPurchase(BuyStockResponseDTO buyStockResponseDTO);
		void ProcessNextDayAccountSale(SellStockResponseDTO sellStockResponseDTO);
	}
}