using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Infrastructure.Constants;
using Hangfire;

namespace API.Settlement.Infrastructure.Services.SettlementServices
{
	public partial class SettlementService
    {
		private decimal CalculateTotalBuyingPriceWithCommission(decimal totalBuyingPriceWithoutCommission)
		{
			return totalBuyingPriceWithoutCommission + (totalBuyingPriceWithoutCommission * InfrastructureConstants.Commission);
		}
		private decimal CalculateUpdatedAccountBalanceForBuy(decimal accountBalance, decimal totalBuyingPriceWithCommission)
		{
			return accountBalance - totalBuyingPriceWithCommission;
		}

		public void PerformBuyStock(BuyStockResponseDTO responseDTO, decimal accountBalance, decimal totalBuyingPriceWithCommission)
		{
			decimal updatedAccountBalance = CalculateUpdatedAccountBalanceForBuy(accountBalance,totalBuyingPriceWithCommission);

			responseDTO.IsSuccessful = true;
			responseDTO.Message = InfrastructureConstants.TransactionSuccessMessage;
			responseDTO.UpdatedAccountBalance = updatedAccountBalance;
        }

	}
}