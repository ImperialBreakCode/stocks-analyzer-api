namespace API.Settlement.Domain.DTOs.Request
{
	public class StockInfoRequestDTO
	{
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePriceExcludingCommission { get; set; }
		public decimal TotalPriceExcludingCommission => Quantity * SinglePriceExcludingCommission;
	}
}