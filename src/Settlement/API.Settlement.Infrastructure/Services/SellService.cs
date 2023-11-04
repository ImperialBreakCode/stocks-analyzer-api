using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
    public class SellService : ISellService
	{
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IHangfireService _hangfireService;
		private readonly IUserDictionaryService _userDictionaryService;


		public SellService(IInfrastructureConstants infrastructureConstants, 
						ITransactionMapperService transactionMapperService, 
						IHangfireService hangfireService, 
						IUserDictionaryService userDictionaryService)
		{
			_infrastructureConstants = infrastructureConstants;
			_transactionMapperService = transactionMapperService;
			_hangfireService = hangfireService;
			_userDictionaryService = userDictionaryService;

		}

		public async Task<ICollection<SellStockResponseDTO>> SellStocks(ICollection<SellStockDTO> sellStocksDTOs)
		{
			var sellStocksResponseDTOs = new List<SellStockResponseDTO>();
			foreach (var sellStockDTO in sellStocksDTOs)
			{
				decimal totalSellingPriceIncludingCommission = CalculateTotalSellingPriceIncludingCommission(sellStockDTO.TotalSellingPriceExcludingCommission);

				var sellStockResponseDTO = _transactionMapperService.CreateSellTransactionResponse(sellStockDTO, totalSellingPriceIncludingCommission);

				sellStocksResponseDTOs.Add(sellStockResponseDTO);

				_hangfireService.ScheduleSellStockJob(sellStockResponseDTO);

				// TODO: Да добавя логиката за изтриване от кеша на продадените акции
				// _userDictionaryService.DeleteStock(...);
			}
			return sellStocksResponseDTOs;
		}

		private decimal CalculateTotalSellingPriceIncludingCommission(decimal totalSellingPriceExcludingCommission)
		{
			return totalSellingPriceExcludingCommission - (totalSellingPriceExcludingCommission * _infrastructureConstants.Commission);
		}

	}
}