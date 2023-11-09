using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class BuyService : IBuyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;


		public BuyService(IHttpClientFactory httpClientFactory,
						IInfrastructureConstants constants,
						ITransactionMapperService buyTransactionMapperService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_transactionMapperService = buyTransactionMapperService;
		}

		public async Task<AvailabilityResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			decimal walletBalance = 1000.5M;//await GetWalletBalance(finalizeTransactionRequestDTO.WalletId);
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, ref walletBalance);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);

			}

			return _transactionMapperService.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

		private decimal CalculatePriceIncludingCommission(decimal totalPriceExcludingCommission)
		{
			return totalPriceExcludingCommission + (totalPriceExcludingCommission * _infrastructureConstants.Commission);
		}
		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, ref decimal walletBalance)
		{
			decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission);
			if (walletBalance < totalPriceIncludingCommission)
			{
				return _transactionMapperService.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			walletBalance -= totalPriceIncludingCommission;
			return _transactionMapperService.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}
		private async Task<decimal> GetWalletBalance(string walletId)
		{
			decimal balance = 0;
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				var response = await _httpClient.GetAsync(_infrastructureConstants.GETWalletBalanceRoute(walletId));
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					balance = JsonConvert.DeserializeObject<decimal>(json);
				}
			}
			return balance;
		}
	}
}