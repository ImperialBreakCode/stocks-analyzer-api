using API.Accounts.Application.DTOs.Enums;

namespace API.Accounts.Application.DTOs.Request
{
    public class DepositWalletDTO
    {
        public decimal Value { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string WalletId { get; set; }
    }
}
