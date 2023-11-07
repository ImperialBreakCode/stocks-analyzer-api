namespace API.Accounts.Application.DTOs.ExternalDTOs
{
    public class StockActionInfo
    {
        public string StockId { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePriceExcludingCommission { get; set; }
    }
}
