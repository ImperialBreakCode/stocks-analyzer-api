namespace API.Accounts.Domain.Entities
{
    public class Stock : BaseEntity
    {
        public int Quantity { get; set; }
        public int WaitingForPurchaseCount { get; set; }
        public int WaitingForSaleCount { get; set; }
        public string StockName { get; set; }
        public string WalletId { get; set; }
    }
}
