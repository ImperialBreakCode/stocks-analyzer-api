using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class BuyService : IBuyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionService _commissionService;

		public BuyService(IHttpClientFactory httpClientFactory,
						IInfrastructureConstants constants,
						IMapperManagementWrapper buyTransactionMapperService,
						IUserCommissionService commissionService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_mapperManagementWrapper = buyTransactionMapperService;
			_commissionService = commissionService;
		}

		public async Task<AvailabilityResponseDTO> BuyStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			//decimal walletBalance = 1000.5M;
			decimal walletBalance = await GetWalletBalance(finalizeTransactionRequestDTO.WalletId);
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, finalizeTransactionRequestDTO.UserRank, ref walletBalance);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);

			}

			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO,UserRank userRank, ref decimal walletBalance)
		{
			decimal totalPriceIncludingCommission = _commissionService.CalculatePriceAfterAddingBuyCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, userRank);
			if (walletBalance < totalPriceIncludingCommission)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			walletBalance -= totalPriceIncludingCommission;
			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
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