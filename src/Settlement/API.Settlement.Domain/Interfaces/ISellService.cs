using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISellService
	{
		Task<IEnumerable<ResponseStockDTO>> SellStocks(IEnumerable<RequestStockDTO> requestStockDTOs);
	}
}