using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Response.LossCheckDTOs
{
	public class WalletLossCheckDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public bool IsSale { get; set; }
		public IEnumerable<StockLossCheckDTO> StockLossCheckDTOs { get; set; }
	}
}
