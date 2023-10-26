namespace API.Accounts.Domain.Entities
{
    public class Stock : BaseEntity
    {
        public int Quantity { get; set; }
        public string StockName { get; set; } 
        public string WalletId { get; set; }
    }
}
