using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Response.LossCheckDTOs
{
	public class StockLossCheckDTO
	{
		public string TransactionId { get; set; }
		public string Message { get; set; }
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePriceExcludingCommission { get; set; }
		public decimal TotalPriceExcludingCommission => Quantity * SinglePriceExcludingCommission;
	}
}
