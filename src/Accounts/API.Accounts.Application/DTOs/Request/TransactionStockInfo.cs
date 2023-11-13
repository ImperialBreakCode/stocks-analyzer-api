namespace API.Accounts.Application.DTOs.Request
{
    public class TransactionStockInfo
    {
        public string Message { get; set; }
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePriceIncludingCommission { get; set; }
    }
}
