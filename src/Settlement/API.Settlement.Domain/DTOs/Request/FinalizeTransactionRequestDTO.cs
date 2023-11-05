namespace API.Settlement.Domain.DTOs.Request
{
	public class FinalizeTransactionRequestDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public bool IsSale { get; set; }
		public IEnumerable<StockInfoRequestDTO> StockInfoRequestDTOs { get; set; }
	}
}
/*
 * Един walletId може да съдържа няколко вида stockId
 */