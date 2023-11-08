using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class SellService : ISellService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;


		public SellService(IHttpClientFactory httpClientFactory,
						IInfrastructureConstants infrastructureConstants,
						ITransactionMapperService transactionMapperService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = infrastructureConstants;
			_transactionMapperService = transactionMapperService;

		}

		public async Task<AvailabilityResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				var stockDTO = await GetStockDTO(_infrastructureConstants.GETStockRoute(stockInfoRequestDTO.StockId));
				decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission);

				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, stockDTO.Quantity, totalPriceIncludingCommission);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);
			}

			return _transactionMapperService.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, int availableQuantity, decimal totalPriceIncludingCommission)
		{
			if (availableQuantity < stockInfoRequestDTO.Quantity)
			{
				return _transactionMapperService.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			return _transactionMapperService.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

		private async Task<StockDTO> GetStockDTO(string uri)
		{
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				var response = await _httpClient.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var stockDTO = JsonConvert.DeserializeObject<StockDTO>(jsonResponse);
					return stockDTO;
				}
			}
			return null;
		}

		private decimal CalculatePriceIncludingCommission(decimal totalPriceExcludingCommission)
		{
			return totalPriceExcludingCommission - (totalPriceExcludingCommission * _infrastructureConstants.Commission);
		}

	}
}