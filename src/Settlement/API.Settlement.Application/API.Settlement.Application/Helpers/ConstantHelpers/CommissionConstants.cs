using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Helpers.ConstantHelpers
{
	public class CommissionConstants : ICommissionConstants
	{
		public decimal BaseCommission => 0.0005M;
		public decimal SpecialTraderCommission => 0.0004M;
		public decimal VipTraderCommission => 0.0003M;
		public decimal GetCommissionBasedOnUserType(UserRank userRank)
		{
			switch (userRank)
			{
				case UserRank.Demo:
				case UserRank.RegularTrader: return BaseCommission;
				case UserRank.SpecialTrader: return SpecialTraderCommission;
				case UserRank.VipTrader: return VipTraderCommission;
				default: throw new Exception("Invalid user type!");
			}
		}

	}
}
