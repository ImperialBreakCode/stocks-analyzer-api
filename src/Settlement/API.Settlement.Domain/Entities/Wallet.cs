namespace API.Settlement.Domain.Entities
{
    public class Wallet
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<Stock> Stocks { get; set; } = Enumerable.Empty<Stock>();
    }
}