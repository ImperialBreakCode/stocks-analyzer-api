using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISettlementServiceWrapper
	{
		IBuyService BuyService { get; }
		ISellService SellService { get; }
		Task<IEnumerable<ResponseStockDTO>> ProcessTransactions(IEnumerable<RequestStockDTO> requestStockDTOs);
	}
}