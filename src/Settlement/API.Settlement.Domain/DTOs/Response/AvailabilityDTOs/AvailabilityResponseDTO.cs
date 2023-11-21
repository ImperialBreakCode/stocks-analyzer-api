namespace API.Settlement.Domain.DTOs.Response.AvailabilityDTOs
{
	public class AvailabilityResponseDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string UserEmail { get; set; }
		public bool IsSale { get; set; }
		public IEnumerable<AvailabilityStockInfoResponseDTO> AvailabilityStockInfoResponseDTOs { get; set; }
		public decimal TotalSuccessfulPrice => AvailabilityStockInfoResponseDTOs.Where(ь => ь.IsSuccessful).Sum(ь => ь.TotalPriceIncludingCommission);
	}
}