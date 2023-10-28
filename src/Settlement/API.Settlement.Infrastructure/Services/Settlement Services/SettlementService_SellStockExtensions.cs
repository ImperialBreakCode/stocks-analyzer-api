using API.Settlement.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService
	{
		private decimal CalculateTotalSellingPriceWithCommission(decimal totalSellingPriceWithoutCommission)
		{
			return totalSellingPriceWithoutCommission * 0.9995m;
		}
		private decimal CalculateUpdatedAccountBalanceForSell(decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			return accountBalance + totalSellingPriceWithCommission;
		}
		public void PerformSellStock(SellStockResponseDTO responseDTO, decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			decimal updatedAccountBalance = accountBalance + totalSellingPriceWithCommission;

			responseDTO.IsSuccessful = true;
			responseDTO.Message = "Transaction accepted!";
			responseDTO.UpdatedAccountBalance = updatedAccountBalance;
		}
		
	}
}
