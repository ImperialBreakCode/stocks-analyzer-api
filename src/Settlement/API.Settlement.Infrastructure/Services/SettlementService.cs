using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using API.Settlement.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services
{
	public class SettlementService : ISettlementService
	{
		private IHttpClient _httpClient;

		public SettlementService(IHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<BuyStockResponseDTO> BuyStock(BuyStockDTO buyStockDTO)
		{
			string accountBalanceAsString = await _httpClient.GetStringAsync(APIAccountRoutes.GetAccountBalance(buyStockDTO.UserId));
			decimal accountBalance = decimal.Parse(accountBalanceAsString);
			decimal totalBuyingPriceWithCommission = buyStockDTO.TotalBuyingPriceWithoutCommission * 1.0005m;
			decimal updatedAccountBalance;


			var responseDTO = new BuyStockResponseDTO();

			if (accountBalance < totalBuyingPriceWithCommission)
			{
				responseDTO.IsSuccessful = false;
				responseDTO.Message = "Transaction declined!";
				responseDTO.UpdatedAccountBalance = accountBalance;
			}
			else
			{
				updatedAccountBalance = accountBalance - totalBuyingPriceWithCommission;

				responseDTO.IsSuccessful = true;
				responseDTO.Message = "Transaction accepted!";
				responseDTO.UpdatedAccountBalance = updatedAccountBalance;
			}

			return responseDTO;
		}

		public async Task<SellStockResponseDTO> SellStock(SellStockDTO sellStockDTO)
		{
			string accountBalanceAsString = await _httpClient.GetStringAsync(APIAccountRoutes.GetAccountBalance(sellStockDTO.UserId));
			decimal accountBalance = decimal.Parse(accountBalanceAsString);
			decimal totalSellingPriceWithCommission = sellStockDTO.TotalSellingPriceWithoutCommission * 0.9995m;
			decimal updatedAccountBalance = accountBalance + totalSellingPriceWithCommission;

			var responseDTO = new SellStockResponseDTO
			{
				IsSuccessful = true,
				Message = "Transaction accepted!",
				UpdatedAccountBalance = updatedAccountBalance
			};

			return responseDTO;
		}

	}
}
