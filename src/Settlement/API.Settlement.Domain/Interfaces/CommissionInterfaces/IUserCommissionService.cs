using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
	public interface IUserCommissionService
	{
		decimal CalculatePriceAfterAddingBuyCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterRemovingBuyCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterAddingSaleCommission(decimal price, UserRank userRank);
		decimal CalculatePriceAfterRemovingSaleCommission(decimal price, UserRank userRank);
		decimal CalculateSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity) => totalPriceIncludingCommission / quantity;
	}
}
