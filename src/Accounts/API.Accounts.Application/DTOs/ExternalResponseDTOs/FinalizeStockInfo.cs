namespace API.Accounts.Application.DTOs.ExternalResponseDTOs
{
    public class FinalizeStockInfo
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePriceIncludingCommission { get; set; }
        public decimal TotalPriceIncludingCommission
            => SinglePriceIncludingCommission * Quantity;
    }
}
