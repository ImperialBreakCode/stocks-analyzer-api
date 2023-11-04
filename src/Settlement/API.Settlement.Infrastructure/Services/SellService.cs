using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Infrastructure.Services
{
	public class SellService : ISellService
	{
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IUserWalletDictionaryService _userDictionaryService;


		public SellService(IInfrastructureConstants infrastructureConstants,
						ITransactionMapperService transactionMapperService,
						IUserWalletDictionaryService userDictionaryService)
		{
			_infrastructureConstants = infrastructureConstants;
			_transactionMapperService = transactionMapperService;
			_userDictionaryService = userDictionaryService;

		}

		public async Task<IEnumerable<ResponseStockDTO>> SellStocks(IEnumerable<RequestStockDTO> requestStockDTOs)
		{
			var responseStockDTOs = new List<ResponseStockDTO>();
			foreach (var requestStockDTO in requestStockDTOs)
			{
				decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(requestStockDTO.TotalPriceExcludingCommission);

				var responseStockDTO = _transactionMapperService.CreateTransactionResponse(requestStockDTO, totalPriceIncludingCommission, Status.Scheduled);

				responseStockDTOs.Add(responseStockDTO);

				var stock = _transactionMapperService.CreateStockDTO(requestStockDTO);
				//_userDictionaryService.CloseWallet(stock.WalletId, stock.StockId);
			}

			return responseStockDTOs;
		}

		private decimal CalculatePriceIncludingCommission(decimal totalPriceExcludingCommission)
		{
			return totalPriceExcludingCommission - (totalPriceExcludingCommission * _infrastructureConstants.Commission);
		}

	}
}