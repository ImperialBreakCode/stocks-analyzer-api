namespace API.Settlement.Domain.DTOs.Request
{
	public class StockDTO
	{
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public string WalletId { get; set; }
	}
}