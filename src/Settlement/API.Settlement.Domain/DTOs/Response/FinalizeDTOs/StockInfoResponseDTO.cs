using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Response
{
	public class StockInfoResponseDTO
	{
		public bool IsSuccessful { get; set; }
		public string Message { get; set; }
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePriceIncludingCommission { get; set; }
		public decimal TotalPriceIncludingCommission { get { return Quantity * SinglePriceIncludingCommission; } }
	}
}
