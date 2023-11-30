using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.EmailServices
{
	public class HTMLContentGenerator : IHTMLContentGenerator
	{
		public string GenerateTransactionSummaryContent(FinalizeTransactionResponseDTO data)
		{
			StringBuilder htmlBuilder = new StringBuilder();

			htmlBuilder.Append("<html><head><title>Transaction Details</title></head><body>");
			htmlBuilder.Append($"<h1>Transaction Details for Wallet ID: {data.WalletId}</h1>");
			htmlBuilder.Append($"<p>User ID: {data.UserId}</p>");
			htmlBuilder.Append($"<p>User Email: {data.UserEmail}</p>");
			htmlBuilder.Append($"<p>Is Sale: {(data.IsSale ? "Yes" : "No")}</p>");
			htmlBuilder.Append($"<p>User Rank: {data.UserRank}</p>");

			if (data.StockInfoResponseDTOs != null && data.StockInfoResponseDTOs.Any())
			{
				htmlBuilder.Append("<h2>Stock Information</h2>");
				htmlBuilder.Append("<table border='1'><tr><th>Transaction ID</th><th>Message</th><th>Stock ID</th><th>Stock Name</th><th>Quantity</th><th>Single Price (Including Commission)</th><th>Total Price (Including Commission)</th></tr>");

				foreach (var stockInfo in data.StockInfoResponseDTOs)
				{
					htmlBuilder.Append("<tr>");
					htmlBuilder.Append($"<td>{stockInfo.TransactionId}</td>");
					htmlBuilder.Append($"<td>{stockInfo.Message}</td>");
					htmlBuilder.Append($"<td>{stockInfo.StockId}</td>");
					htmlBuilder.Append($"<td>{stockInfo.StockName}</td>");
					htmlBuilder.Append($"<td>{stockInfo.Quantity}</td>");
					htmlBuilder.Append($"<td>{stockInfo.SinglePriceIncludingCommission}</td>");
					htmlBuilder.Append($"<td>{stockInfo.TotalPriceIncludingCommission}</td>");
					htmlBuilder.Append("</tr>");
				}

				htmlBuilder.Append("</table>");
			}
			else
			{
				htmlBuilder.Append("<p>No stock information available.</p>");
			}

			htmlBuilder.Append("</body></html>");

			return htmlBuilder.ToString();
		}
	}
}
