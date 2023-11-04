namespace API.Settlement.Domain.DTOs.Request
{
	public class SellStockDTO
	{
		public string UserId { get; set; }
		public string StockId { get; set; }
		public decimal TotalSellingPriceExcludingCommission { get; set; }
	}
}