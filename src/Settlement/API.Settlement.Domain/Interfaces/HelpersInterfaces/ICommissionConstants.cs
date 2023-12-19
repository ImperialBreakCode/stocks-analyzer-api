using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface ICommissionConstants
	{
		public decimal BaseCommission { get; }
		public decimal SpecialTraderCommission { get; }
		public decimal VipTraderCommission { get; }
		public decimal GetCommissionBasedOnUserType(UserRank userRank);
	}
}
