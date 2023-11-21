using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities
{
	public class Stock
	{
		[BsonId]
		[BsonElement("_id")]
		public string StockId { get; set; }

		[BsonElement("stock_name")]
		public string StockName { get; set; }

		[BsonElement("invested_amount")]
		public decimal InvestedAmount { get; set; }

		[BsonElement("average_single_stock_price")]
		public decimal AverageSingleStockPrice { get; set; }

		[BsonElement("quantity")]
		public int Quantity { get; set; }
	}
}
