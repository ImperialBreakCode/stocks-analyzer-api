using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Infrastructure.Constants;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService
    {
		private decimal CalculateTotalBuyingPriceWithCommission(decimal totalBuyingPriceWithoutCommission)
		{
			return totalBuyingPriceWithoutCommission + (totalBuyingPriceWithoutCommission * InfrastructureConstants.Commission);
		}
		private decimal CalculateUpdatedAccountBalanceForBuy(decimal accountBalance, decimal totalSellingPriceWithCommission)
		{
			return accountBalance - totalSellingPriceWithCommission;
		}
		public void PerformBuyStock(BuyStockResponseDTO responseDTO, decimal accountBalance, decimal totalBuyingPriceWithCommission)
		{
			decimal updatedAccountBalance = accountBalance - totalBuyingPriceWithCommission;

			responseDTO.IsSuccessful = true;
			responseDTO.Message = InfrastructureConstants.TransactionSuccessMessage;
			responseDTO.UpdatedAccountBalance = updatedAccountBalance;
        }

	}
}