namespace API.Accounts.Application.DTOs.Request
{
    public class DepositWalletDTO
    {
        public decimal Value { get; set; }
        public string CurrencyType { get; set; }
        public string WalletId { get; set; }
    }
}
