namespace API.Settlement.Domain.DTOs.Request
{
	public class RequestStockDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string StockId { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePrice { get; set; }
		public bool IsSale { get; set; }
		public decimal TotalPriceExcludingCommission { get { return Quantity * SinglePrice; } }
	}
}