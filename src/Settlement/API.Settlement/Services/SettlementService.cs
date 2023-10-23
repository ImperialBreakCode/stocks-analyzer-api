using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
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
			decimal accountBalance = decimal.Parse(await _httpClient.GetStringAsync($"api/accounts/{buyStockDTO.UserId}/balance"));
			decimal totalBuyingPriceWithCommission = buyStockDTO.TotalBuyingPriceWithoutCommission * 1.05m;

			var responseDTO = new BuyStockResponseDTO();

			if (accountBalance < totalBuyingPriceWithCommission)
			{
				responseDTO.IsSuccessful = false;
				responseDTO.Message = "Transaction declined!";
			}
			else
			{
				responseDTO.IsSuccessful = true;
				responseDTO.Message = "Transaction accepted!";
			}

			return responseDTO;
		}

		public async Task<SellStockResponseDTO> SellStock(SellStockDTO sellStockDTO)
		{
			decimal accountBalance = decimal.Parse(await _httpClient.GetStringAsync($"api/accounts/{sellStockDTO.UserId}/balance"));
			decimal totalSellingPriceWithCommission = sellStockDTO.TotalSellingPriceWithoutCommission * 0.05m;
			decimal updatedAccountBalance = accountBalance + totalSellingPriceWithCommission;

			var responseDTO = new SellStockResponseDTO
			{
				IsSuccessful = true,
				Message = "Transaction accepted!",
				UpdatedAccountBalance = updatedAccountBalance
			};

			return responseDTO;
		}
		
		/*
		private async Task SendMessageToUserAccount(string userId, string message)
		{
			await _httpClient.PostAsync($"api/accounts/{userId}/messages", message);
		}

		private async Task CloseExposure(string userId)
		{
			await _httpClient.DeleteAsync($"api/accounts/{userId}/exposure");
		}
		*/

	}
}
