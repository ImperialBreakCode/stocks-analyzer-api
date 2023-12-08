using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces.CommissionInterfaces
{
	public interface IUserCommissionCalculatorHelper
	{
		decimal CalculatePriceAfterAddingBuyCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterRemovingBuyCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterAddingSaleCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterRemovingSaleCommission(decimal price, UserRank userRank);
		decimal CalculateSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity) => totalPriceIncludingCommission / quantity;
	}
}
