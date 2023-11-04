namespace API.Settlement.Domain.DTOs.Response
{
	public class BuyStockResponseDTO
	{
		public bool IsSuccessful { get; set; } = false;
		public string Message { get; set; }
		public string UserId { get; set; }
		public string StockId { get; set; }
		public decimal TotalBuyingPriceIncludingCommission { get; set; }
	}
}