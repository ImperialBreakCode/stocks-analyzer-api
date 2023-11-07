namespace API.Settlement.Domain.Entities
{
    public class Wallet
    {
        public string WalletId { get; set; }
        public string StockId { get; set; }
        public string StockName {  get; set; }
        public decimal InvestedAmount { get; set; }
        public int Quantity { get; set; }
    }
}