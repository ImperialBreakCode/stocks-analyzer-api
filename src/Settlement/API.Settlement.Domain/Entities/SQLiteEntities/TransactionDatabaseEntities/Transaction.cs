using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities
{
	public class Transaction
	{
		public string TransactionId { get; set; }
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public UserRank UserRank { get; set; }
		public bool IsSale { get; set; }
		public string Message { get; set; }
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPriceIncludingCommission { get; set; }
		public decimal SinglePriceIncludingCommission => TotalPriceIncludingCommission / Quantity;
	}
}
