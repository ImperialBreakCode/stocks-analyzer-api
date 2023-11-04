using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Infrastructure.Services
{
	public class BuyService : IBuyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IUserWalletDictionaryService _userDictionaryService;


		public BuyService(IHttpClientFactory httpClientFactory, IInfrastructureConstants constants, ITransactionMapperService buyTransactionMapperService, IHangfireService hangfireService, IUserWalletDictionaryService userDictionaryService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_transactionMapperService = buyTransactionMapperService;
			_userDictionaryService = userDictionaryService;
		}

		public async Task<IEnumerable<ResponseStockDTO>> BuyStocks(IEnumerable<RequestStockDTO> requestStockDTOs)
		{
			var responseStockDTOs = new List<ResponseStockDTO>();
			foreach (var requestStockDTO in requestStockDTOs)
			{
				decimal walletBalance = 1000.50M;//await GetWalletBalance(requestStockDTO.WalletId);
				decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(requestStockDTO.TotalPriceExcludingCommission);

				var responseStockDTO = new ResponseStockDTO();
				if (walletBalance < totalPriceIncludingCommission)
				{
					responseStockDTO = _transactionMapperService.CreateTransactionResponse(requestStockDTO, totalPriceIncludingCommission, Status.Declined);
				}
				else
				{
					responseStockDTO = _transactionMapperService.CreateTransactionResponse(requestStockDTO, totalPriceIncludingCommission, Status.Scheduled);

					var stock = _transactionMapperService.CreateStockDTO(requestStockDTO);
					//_userDictionaryService.CreateOrUpdateWallet(stock);
				}

				responseStockDTOs.Add(responseStockDTO);
			}

			return responseStockDTOs;
		}

		private decimal CalculatePriceIncludingCommission(decimal totalPriceExcludingCommission)
		{
			return totalPriceExcludingCommission + (totalPriceExcludingCommission * _infrastructureConstants.Commission);
		}

		private async Task<decimal> GetWalletBalance(string walletId)
		{
			decimal balance = 0;
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				balance = decimal.Parse(await _httpClient.GetStringAsync(_infrastructureConstants.GetWalletBalanceRoute(walletId)));
			}
			return balance;
		}

	}
}