using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
