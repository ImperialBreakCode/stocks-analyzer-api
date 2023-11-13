using System.ComponentModel.DataAnnotations;

namespace API.Accounts.Application.DTOs.Request
{
    public class DepositWalletDTO
    {
        [Required]
        public decimal Value { get; set; }

        [Required]
        public string CurrencyType { get; set; }
    }
}
