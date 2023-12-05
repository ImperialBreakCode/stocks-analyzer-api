namespace API.Accounts.Application.DTOs.RabbitMQConsumerDTOs
{
    public class TransactionConsumeDTO
    {
        public string TransactionId { get; set; }
        public string WalletId { get; set; }
        public string StockId { get; set; }
        public bool IsSale { get; set; }
        public string Message { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPriceIncludingCommission { get; set; }
    }
}
