using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Request
{
	public class StockInfoRequestDTO
	{
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePriceExcludingCommission { get; set; }
		public decimal TotalPriceExcludingCommission { get { return Quantity * SinglePriceExcludingCommission; } }
	}
}