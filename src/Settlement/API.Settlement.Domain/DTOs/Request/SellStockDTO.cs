using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Request
{
	public class SellStockDTO
	{
		public string UserId { get; set; }
		public string StockId { get; set;}
		public decimal TotalSellingPriceWithoutCommission { get; set; }
	}
}