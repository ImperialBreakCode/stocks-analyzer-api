using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface IRouteConstants
	{
		public string BaseAccountAPIHost { get; }
		public string BaseStockAPIHost { get; }
		public string GETWalletBalanceRoute(string walletId);
		public string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
		public string GETStockRoute(string stockId);
		public string GETStockPriceRoute(string stockName);
	}
}
