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
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionService _commissionService;

		public BuyService(IHttpClientFactory httpClientFactory,
						  IInfrastructureConstants constants,
						  IMapperManagementWrapper mapperManagementWrapper,
						  IUserCommissionService commissionService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_mapperManagementWrapper = mapperManagementWrapper;
			_commissionService = commissionService;
		}

		public async Task<AvailabilityResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = await GenerateAvailabilityStockInfoList(finalizeTransactionRequestDTO);

			return MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}


		private async Task<IEnumerable<AvailabilityStockInfoResponseDTO>> GenerateAvailabilityStockInfoList(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			//decimal walletBalance = await GetWalletBalance(finalizeTransactionRequestDTO.WalletId);
			decimal walletBalance = 1000.5M; //Hardcoded for testing!

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
				var response = await _httpClient.GetAsync(_infrastructureConstants.GETWalletBalanceRoute(walletId));
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
			decimal totalPriceIncludingCommission = CalculateTotalPriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, userRank);
			if (walletBalance < totalPriceIncludingCommission)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			walletBalance -= totalPriceIncludingCommission;
			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

		private decimal CalculateTotalPriceIncludingCommission(decimal totalPriceExcludingCommission, UserRank userRank)
		{
			return _commissionService.CalculatePriceAfterAddingBuyCommission(totalPriceExcludingCommission, userRank);
		}

		private AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}
	}
}