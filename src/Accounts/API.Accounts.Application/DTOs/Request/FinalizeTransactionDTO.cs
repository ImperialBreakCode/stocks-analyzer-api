namespace API.Accounts.Application.DTOs.Request
{
    public class FinalizeTransactionDTO
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public ICollection<TransactionStockInfo> StockInfoResponseDTOs { get; set; }
    }
}
