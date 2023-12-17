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
	public class BuyService : IBuyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConstantsHelperWrapper _infrastructureConstants;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionCalculatorHelper _userCommissionCalculatorHelper;

		public BuyService(IHttpClientFactory httpClientFactory,
						  IConstantsHelperWrapper constants,
						  IMapperManagementWrapper mapperManagementWrapper,
						  IUserCommissionCalculatorHelper userCommissionCalculatorHelper)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_mapperManagementWrapper = mapperManagementWrapper;
			_userCommissionCalculatorHelper = userCommissionCalculatorHelper;
		}

		public async Task<AvailabilityResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = await GenerateAvailabilityStockInfoList(finalizeTransactionRequestDTO);

			var availabilityResponseDTO = _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
			return availabilityResponseDTO;
		}


		private async Task<IEnumerable<AvailabilityStockInfoResponseDTO>> GenerateAvailabilityStockInfoList(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			decimal walletBalance = await GetWalletBalance(finalizeTransactionRequestDTO.WalletId);
			//decimal walletBalance = 1000.5M; //TODO: Hardcoded for testing!

			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, finalizeTransactionRequestDTO.UserRank, ref walletBalance);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);
			}
			return availabilityStockInfoResponseDTOs;
		}

		private async Task<decimal> GetWalletBalance(string walletId)
		{
			decimal balance = 0;
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				var response = await _httpClient.GetAsync(_infrastructureConstants.RouteConstants.GETWalletBalanceRoute(walletId));
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					balance = JsonConvert.DeserializeObject<decimal>(json);
				}
			}
			return balance;
		}

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, UserRank userRank, ref decimal walletBalance)
		{
			decimal totalPriceIncludingCommission = _userCommissionCalculatorHelper.CalculatePriceAfterAddingBuyCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, userRank);
			if (walletBalance < totalPriceIncludingCommission)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			walletBalance -= totalPriceIncludingCommission;
			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

	}
}