namespace API.Accounts.Domain.Entities
{
    public class Stock : BaseEntity
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string WalletId { get; set; }
    }
}
