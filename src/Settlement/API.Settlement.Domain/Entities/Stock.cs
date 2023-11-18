using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities
{
	public class Stock
	{
		public string StockId { get; set; }
		public string StockName { get; set; }
		public decimal InvestedAmount { get; set; }
		public decimal InvestedAmountPerStock { get; set; }
		public decimal AverageSingleStockPrice { get; set; }
		public int Quantity { get; set; }
	}
}
