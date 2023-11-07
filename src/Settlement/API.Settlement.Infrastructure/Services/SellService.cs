using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
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

		public async Task<FinalizeTransactionResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
				var stockInfoResponseDTOs = new List<StockInfoResponseDTO>();
				foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
				{
					var stockInfoResponseDTO = new StockInfoResponseDTO();
					var stockDTO = await GetStockDTO(_infrastructureConstants.GETStockRoute(stockInfoRequestDTO.StockId));
					decimal totalPriceIncludingCommission = CalculatePriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission);
					if (stockDTO.Quantity < stockInfoRequestDTO.Quantity)
					{
						stockInfoResponseDTO = _transactionMapperService.MapToStockResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
					}
					else
					{
						stockInfoResponseDTO = _transactionMapperService.MapToStockResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Success);
						//var stock = _transactionMapperService.CreateStockDTO();
						//TODO: _userDictionaryService.RemoveStock(stockDTO.StockId);
					}

					stockInfoResponseDTOs.Add(stockInfoResponseDTO);
				}

				finalizeTransactionResponseDTO = _transactionMapperService.MapToFinalizeTransactionResponseDTO(finalizeTransactionRequestDTO, stockInfoResponseDTOs);

			return finalizeTransactionResponseDTO;
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