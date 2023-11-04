namespace API.Settlement.Domain.DTOs.Request
{
	public class BuyStockDTO
	{
		public string UserId { get; set; }
		public string StockId { get; set; }
		public decimal TotalBuyingPriceExcludingCommission { get; set; }
	}
}