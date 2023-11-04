using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISettlementServiceWrapper
	{
		IBuyService BuyService { get; }
		ISellService SellService { get; }
		Task<ICollection<BuyStockResponseDTO>> BuyStocks(ICollection<BuyStockDTO> buyStockDTOs);
		Task<ICollection<SellStockResponseDTO>> SellStocks(ICollection<SellStockDTO> sellStocksDTOs);
	}
}