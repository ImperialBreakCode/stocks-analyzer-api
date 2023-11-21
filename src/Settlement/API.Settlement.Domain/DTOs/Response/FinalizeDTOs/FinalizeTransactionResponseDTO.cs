namespace API.Settlement.Domain.DTOs.Response
{
	public class FinalizeTransactionResponseDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string UserEmail { get; set; }

		public bool IsSale { get; set; }
		public IEnumerable<StockInfoResponseDTO> StockInfoResponseDTOs { get; set; }
	}
}