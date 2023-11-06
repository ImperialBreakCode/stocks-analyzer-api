namespace API.Accounts.Application.DTOs.Request
{
    public class TransactionStockInfo
    {
        public string StockId { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePriceIncludingCommission { get; set; }
    }
}
