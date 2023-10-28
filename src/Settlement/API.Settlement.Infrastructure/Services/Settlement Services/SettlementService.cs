using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using API.Settlement.Infrastructure.Helpers;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService : ISettlementService
	{
		private IHttpClient _httpClient;

		public SettlementService(IHttpClient httpClient)
		{
			_httpClient = httpClient;
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
				responseDTO.Message = "Transaction declined!";
				responseDTO.UpdatedAccountBalance = accountBalance;
			}
			else
			{
				responseDTO.IsSuccessful = false;
				responseDTO.Message = "Transaction scheduled for execution at 00:01:00 next day.";
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
				Message = "Transaction scheduled for execution at 00:01:00 next day.",
				UpdatedAccountBalance = null
			};
			BackgroundJob.Schedule(() => PerformSellStock(responseDTO, accountBalance, totalSellingPriceWithCommission), GetTimeSpanToNextExecution());
			return responseDTO;
		}



	}
}