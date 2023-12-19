using System.ComponentModel.DataAnnotations;

namespace API.Gateway.Domain.DTOs
{
	public class DepositWalletDTO
	{
		[Required]
		public decimal Value { get; set; }

		[Required]
		public string CurrencyType { get; set; }
	}
}
