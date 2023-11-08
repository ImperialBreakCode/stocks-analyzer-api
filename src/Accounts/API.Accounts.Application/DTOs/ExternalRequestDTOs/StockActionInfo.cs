namespace API.Accounts.Application.DTOs.ExternalRequestDTOs
{
    public class StockActionInfo
    {
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePriceExcludingCommission { get; set; }
    }
}
