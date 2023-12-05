using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services
{
	public class UserCommissionService : IUserCommissionService
	{
		private readonly IInfrastructureConstants _infrastructureConstants;

		public UserCommissionService(IInfrastructureConstants infrastructureConstants)
		{
			_infrastructureConstants = infrastructureConstants;
		}

		public decimal CalculatePriceAfterAddingBuyCommission(decimal price, UserRank userRank)
		{
			return price + (price * _infrastructureConstants.GetCommissionBasedOnUserType(userRank));
		}

		public decimal CalculatePriceAfterAddingSaleCommission(decimal price, UserRank userRank)
		{
			return price - (price * _infrastructureConstants.GetCommissionBasedOnUserType(userRank));
		}

		public decimal CalculatePriceAfterRemovingBuyCommission(decimal price, UserRank userRank)
		{
			return price / (1 + _infrastructureConstants.GetCommissionBasedOnUserType(userRank));
		}

		public decimal CalculatePriceAfterRemovingSaleCommission(decimal price, UserRank userRank)
		{
			return price / (1 - _infrastructureConstants.GetCommissionBasedOnUserType(userRank));
		}

		public decimal CalculateSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity)
		{
			return totalPriceIncludingCommission / quantity;
		}

	}
}