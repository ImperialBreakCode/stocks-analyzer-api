using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities
{
	public class Transaction
	{
		public string TransactionId { get; set; }
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string UserEmail { get; set; }
		public UserRank UserRank { get; set; }
		public bool IsSale { get; set; }
		public string Message { get; set; }
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPriceIncludingCommission { get; set; }
		public decimal SinglePriceIncludingCommission => TotalPriceIncludingCommission / Quantity;
	}
}
