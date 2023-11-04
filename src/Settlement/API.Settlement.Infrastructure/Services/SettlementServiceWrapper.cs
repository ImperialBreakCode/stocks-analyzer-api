using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementServiceWrapper : ISettlementServiceWrapper
	{
		private readonly IHangfireService _hangfireService;
		public IBuyService BuyService { get; }
		public ISellService SellService { get; }

		public SettlementServiceWrapper(IBuyService buyService, ISellService sellService, IHangfireService hangfireService)
		{
			BuyService = buyService;
			SellService = sellService;
			_hangfireService = hangfireService;


		}

		public async Task<IEnumerable<ResponseStockDTO>> ProcessTransactions(IEnumerable<RequestStockDTO> requestStockDTOs)
		{
			var requestBuyStockDTOs = requestStockDTOs.Where(x => !x.IsSale);
			var requestSellStockDTOs = requestStockDTOs.Where(x => x.IsSale);

			var responseBuyStockDTOs = await BuyService.BuyStocks(requestBuyStockDTOs);
			var responseSellStockDTOs = await SellService.SellStocks(requestSellStockDTOs);

			var responseStockDTOs = responseBuyStockDTOs.Concat(responseSellStockDTOs);

			_hangfireService.ScheduleStockProcessingJob(responseStockDTOs);

			return responseStockDTOs;
		}


	}
}