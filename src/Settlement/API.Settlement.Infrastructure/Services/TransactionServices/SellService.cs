using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class SellService : ISellService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionService _commissionService;


		public SellService(IHttpClientFactory httpClientFactory, 
						   IMapperManagementWrapper mapperManagementWrapper, 
						   IUserCommissionService commissionService)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_commissionService = commissionService;
		}

		public async Task<AvailabilityResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = await ProcessStockSellRequests(finalizeTransactionRequestDTO);

			return MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

		private async Task<IEnumerable<AvailabilityStockInfoResponseDTO>> ProcessStockSellRequests(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
            foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
            {
				//var stockDTO = await GetStockDTO(_infrastructureConstants.GETStockRoute(stockInfoRequestDTO.StockId));
				var stockDTO = new StockDTO { Quantity = 1, StockId = "1", StockName = "mc", WalletId = "1" };

				decimal totalPriceIncludingCommission = CalculateTotalPriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, finalizeTransactionRequestDTO.UserRank);

				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, stockDTO.Quantity, totalPriceIncludingCommission);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);
			}
			return availabilityStockInfoResponseDTOs;
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

		private decimal CalculateTotalPriceIncludingCommission(decimal totalPriceExcludingCommission, UserRank userRank)
		{
			return _commissionService.CalculatePriceAfterAddingSaleCommission(totalPriceExcludingCommission, userRank);
		}

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, int availableQuantity, decimal totalPriceIncludingCommission)
		{
			if (availableQuantity < stockInfoRequestDTO.Quantity)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

		private AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

	}
}