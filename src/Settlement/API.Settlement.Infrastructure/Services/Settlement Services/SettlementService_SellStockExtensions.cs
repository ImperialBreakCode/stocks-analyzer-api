using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Infrastructure.Constants;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService
	{
		private decimal CalculateTotalSellingPriceWithCommission(decimal totalSellingPriceWithoutCommission)
		{
			return totalSellingPriceWithoutCommission + (totalSellingPriceWithoutCommission * InfrastructureConstants.Commission);
		}
		private decimal CalculateUpdatedAccountBalanceForSell(decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			return accountBalance + totalSellingPriceWithCommission;
		}
		public void PerformSellStock(SellStockResponseDTO responseDTO, decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			decimal updatedAccountBalance = accountBalance + totalSellingPriceWithCommission;

			responseDTO.IsSuccessful = true;
			responseDTO.Message = InfrastructureConstants.TransactionSuccessMessage;
			responseDTO.UpdatedAccountBalance = updatedAccountBalance;
		}
		
	}
}