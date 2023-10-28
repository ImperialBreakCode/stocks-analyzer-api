using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using API.Settlement.Infrastructure.Constants;
using Hangfire;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService : ISettlementService
	{
		private readonly IHttpClientService _httpClientService;
		private readonly IDateTimeService _dateTimeService;

		public SettlementService(IHttpClientService httpClientService, IDateTimeService dateTimeService)
		{
			_httpClientService = httpClientService;
			_dateTimeService = dateTimeService;
		}

		public async Task<BuyStockResponseDTO> BuyStock(BuyStockDTO buyStockDTO)
		{
			decimal accountBalance = await GetAccountBalance(buyStockDTO.UserId);
			decimal totalBuyingPriceWithCommission = CalculateTotalBuyingPriceWithCommission(buyStockDTO.TotalBuyingPriceWithoutCommission);
			decimal updatedAccountBalance = CalculateUpdatedAccountBalanceForBuy(accountBalance, totalBuyingPriceWithCommission);

			var responseDTO = new BuyStockResponseDTO();

			if (accountBalance < totalBuyingPriceWithCommission)
			{
				responseDTO.IsSuccessful = false;
				responseDTO.Message = InfrastructureConstants.TransactionDeclinedMessage;
				responseDTO.UpdatedAccountBalance = accountBalance;
			}
			else
			{
				responseDTO.IsSuccessful = false;
				responseDTO.Message = InfrastructureConstants.TransactionScheduledMessage;
				responseDTO.UpdatedAccountBalance = null;

				BackgroundJob.Schedule(() => PerformBuyStock(responseDTO, accountBalance, totalBuyingPriceWithCommission), GetTimeSpanToNextExecution());
			}

			return responseDTO;
		}

		public async Task<SellStockResponseDTO> SellStock(SellStockDTO sellStockDTO)
		{
			decimal accountBalance = await GetAccountBalance(sellStockDTO.UserId);
			decimal totalSellingPriceWithCommission = CalculateTotalSellingPriceWithCommission(sellStockDTO.TotalSellingPriceWithoutCommission);
			decimal updatedAccountBalance = CalculateUpdatedAccountBalanceForSell(accountBalance, totalSellingPriceWithCommission);

			var responseDTO = new SellStockResponseDTO
			{
				IsSuccessful = false,
				Message = InfrastructureConstants.TransactionScheduledMessage,
				UpdatedAccountBalance = null
			};
			BackgroundJob.Schedule(() => PerformSellStock(responseDTO, accountBalance, totalSellingPriceWithCommission), GetTimeSpanToNextExecution());
			return responseDTO;
		}



	}
}