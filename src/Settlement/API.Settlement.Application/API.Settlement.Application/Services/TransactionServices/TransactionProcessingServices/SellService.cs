using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.CommissionInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces;
using Newtonsoft.Json;

namespace API.Settlement.Application.Services.TransactionServices.OrderProcessingServices
{
	public class SellService : ISellService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionCalculatorHelper _userCommissionCalculatorHelper;
		private readonly IConstantsHelperWrapper _infrastructureConstants;


		public SellService(IHttpClientFactory httpClientFactory,
						   IMapperManagementWrapper mapperManagementWrapper,
						   IUserCommissionCalculatorHelper userCommissionCalculatorHelper,
						   IConstantsHelperWrapper infrastructureConstants)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_userCommissionCalculatorHelper = userCommissionCalculatorHelper;
			_infrastructureConstants = infrastructureConstants;
		}

		public async Task<AvailabilityResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = await GenerateAvailabilityStockInfoResponseList(finalizeTransactionRequestDTO);

			var availabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
			return availabilityResponseDTO;
		}

		private async Task<IEnumerable<AvailabilityStockInfoResponseDTO>> GenerateAvailabilityStockInfoResponseList(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				var stockDTO = await GetStockDTO(_infrastructureConstants.RouteConstants.GETStockRoute(stockInfoRequestDTO.StockId));//TODO: ?
				//var stockDTO = new StockDTO { Quantity = 1, StockId = "1", StockName = "mc", WalletId = "1" }; //TODO: Hardcoded for testing!

				decimal totalPriceIncludingCommission = _userCommissionCalculatorHelper.CalculatePriceAfterAddingSaleCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, finalizeTransactionRequestDTO.UserRank);

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

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, int availableQuantity, decimal totalPriceIncludingCommission)
		{
			if (availableQuantity < stockInfoRequestDTO.Quantity)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

	}
}