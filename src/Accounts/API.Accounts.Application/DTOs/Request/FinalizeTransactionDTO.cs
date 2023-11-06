namespace API.Accounts.Application.DTOs.Request
{
    public class FinalizeTransactionDTO
    {
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public ICollection<TransactionStockInfo> Stocks { get; set; }
    }
}
