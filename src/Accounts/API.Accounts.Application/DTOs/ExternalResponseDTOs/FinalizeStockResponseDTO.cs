namespace API.Accounts.Application.DTOs.ExternalResponseDTOs
{
    public class FinalizeStockResponseDTO
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public ICollection<FinalizeStockInfo> StockInfoResponseDTOs { get; set; }
        public decimal TotalSuccessfulPrice
            => StockInfoResponseDTOs.Where(s => s.IsSuccessful).Sum(s => s.TotalPriceIncludingCommission);
    }
}
