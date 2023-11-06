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


		public BuyService(IHttpClientFactory httpClientFactory,
						IInfrastructureConstants constants,
						ITransactionMapperService buyTransactionMapperService
			)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_transactionMapperService = buyTransactionMapperService;
		}

		public async Task<FinalizeTransactionResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
				decimal walletBalance = await GetWalletBalance(finalizeTransactionRequestDTO.WalletId);
				var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
				var stockInfoResponseDTOs = new List<StockInfoResponseDTO>();
				foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
				{
					var stockInfoResponseDTO = new StockInfoResponseDTO();
					decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission);
					if (walletBalance < totalPriceIncludingCommission)
					{
						stockInfoResponseDTO = _transactionMapperService.MapToStockResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
					}
					else
					{
						walletBalance -= totalPriceIncludingCommission;
						stockInfoResponseDTO = _transactionMapperService.MapToStockResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Success);
						//var stock = _transactionMapperService.CreateStockDTO();
						//TODO: _userDictionaryService.CreateOrUpdateWallet(stock);
					}

					stockInfoResponseDTOs.Add(stockInfoResponseDTO);
				}

				finalizeTransactionResponseDTO = _transactionMapperService.MapToFinalizeTransactionResponseDTO(finalizeTransactionRequestDTO, stockInfoResponseDTOs);

			return finalizeTransactionResponseDTO;
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
				balance = decimal.Parse(await _httpClient.GetStringAsync(_infrastructureConstants.GETWalletBalanceRoute(walletId)));
			}
			return balance;
		}

	}
}