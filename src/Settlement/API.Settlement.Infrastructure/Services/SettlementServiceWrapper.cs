using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementServiceWrapper : ISettlementServiceWrapper
	{
		public IBuyService BuyService { get; }
		public ISellService SellService { get; }

		public SettlementServiceWrapper(IBuyService buyService, ISellService sellService)
		{
			BuyService = buyService;
			SellService = sellService;
		}
		public async Task<ICollection<BuyStockResponseDTO>> BuyStocks(ICollection<BuyStockDTO> buyStocksDTOs)
		{
			return await BuyService.BuyStocks(buyStocksDTOs);
		}
		public async Task<ICollection<SellStockResponseDTO>> SellStocks(ICollection<SellStockDTO> sellStocksDTO)
		{
			return await SellService.SellStocks(sellStocksDTO);
		}

	}
}