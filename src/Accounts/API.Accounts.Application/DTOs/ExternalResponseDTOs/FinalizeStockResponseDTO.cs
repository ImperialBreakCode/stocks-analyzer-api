namespace API.Accounts.Application.DTOs.ExternalResponseDTOs
{
    public class FinalizeStockResponseDTO
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public ICollection<FinalizeStockInfoResponse> AvailabilityStockInfoResponseDTOs { get; set; }
        public decimal TotalSuccessfulPrice
            => AvailabilityStockInfoResponseDTOs.Where(s => s.IsSuccessful).Sum(s => s.TotalPriceIncludingCommission);
    }
}
