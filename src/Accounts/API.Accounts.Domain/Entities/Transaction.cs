namespace API.Accounts.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Transaction()
        {
            Date = DateTime.UtcNow;
        }

        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string StockId { get; set; }
        public string Walletid { get; set; }
    }
}
