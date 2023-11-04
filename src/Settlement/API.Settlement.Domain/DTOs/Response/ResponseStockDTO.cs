namespace API.Settlement.Domain.DTOs.Response
{
	public class ResponseStockDTO
	{
		public bool IsSuccessful { get; set; }
		public string Message { get; set; }
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string StockId { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePrice { get; set; }
		public bool IsSale { get; set; }
		public decimal TotalPriceIncludingCommission { get; set; }
	}
}