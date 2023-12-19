using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Helpers.ConstantHelpers
{
	public class RouteConstants : IRouteConstants
	{
		public string BaseAccountAPIHost =>  "https://localhost:5032"; //TODO: ?
		public string BaseStockAPIHost => "https://localhost:5031"; //TODO: ?
		public string GETWalletBalanceRoute(string walletId)
			=> $"{BaseAccountAPIHost}/api/Wallet/GetWalletBalance/{walletId}";
		public string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
			=> $"{BaseAccountAPIHost}/api/Transaction/CompleteTransaction";
		public string GETStockRoute(string stockId)
			=> $"{BaseAccountAPIHost}/api/Stock/GetStock/{stockId}";
		public string GETStockPriceRoute(string stockName)
			=> $"{BaseStockAPIHost}/api/Stock/current/{stockName}";

	}
}
