namespace API.Settlement.DTOs.Request
{
	public class BuyStockDTO
	{
		public string UserId { get; set; }
		public string StockId { get; set; }
		public decimal TotalBuyingPriceWithoutCommission { get; set; }
	}
}
