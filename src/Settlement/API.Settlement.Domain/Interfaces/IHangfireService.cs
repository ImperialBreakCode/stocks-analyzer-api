using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IHangfireService
	{
		void ScheduleBuyStockJob(BuyStockResponseDTO buyStockResponseDTO);
		void ScheduleSellStockJob(SellStockResponseDTO sellStockResponseDTO);
	}
}