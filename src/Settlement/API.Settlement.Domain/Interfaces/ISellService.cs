using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISellService
	{
		Task<ICollection<SellStockResponseDTO>> SellStocks(ICollection<SellStockDTO> sellStocksDTOs);
	}
}