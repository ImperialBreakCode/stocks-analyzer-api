using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService
    {
		private decimal CalculateTotalBuyingPriceWithCommission(decimal totalBuyingPriceWithoutCommission)
		{
			return totalBuyingPriceWithoutCommission * 1.0005m;
		}
		private decimal CalculateUpdatedAccountBalanceForBuy(decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			return accountBalance - totalSellingPriceWithCommission;
		}
		public void PerformBuyStock(BuyStockResponseDTO responseDTO, decimal accountBalance, decimal totalBuyingPriceWithCommission)
		{
			decimal updatedAccountBalance = accountBalance - totalBuyingPriceWithCommission;

			responseDTO.IsSuccessful = true;
			responseDTO.Message = "Transaction accepted!";
			responseDTO.UpdatedAccountBalance = updatedAccountBalance;
        }

	}
}
