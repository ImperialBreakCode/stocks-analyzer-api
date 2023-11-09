using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Helpers.Constants
{
	public class InfrastructureConstants : IInfrastructureConstants
	{
		private static readonly string _transactionDeclinedMessage = "Transaction declined: Insufficient resources.";
		private static readonly string _transactionScheduledMessage = "Transaction scheduled for execution tomorrow at 00:01:00.";
		private static readonly string _transactionSuccessMessage = "Transaction completed successfully.";
		private static readonly decimal _commission = 0.0005M;
		private static readonly string _baseAccountHost = "https://localhost:5032";
		public string TransactionDeclinedMessage => _transactionDeclinedMessage;

		public string TransactionScheduledMessage => _transactionScheduledMessage;

		public string TransactionSuccessMessage => _transactionSuccessMessage;

		public decimal Commission => _commission;
		public string BaseAccountHost => _baseAccountHost;

		public string GETWalletBalanceRoute(string walletId)
			=> $"{BaseAccountHost}/api/Wallet/GetWalletBalance/{walletId}";
		public string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
			=> $"{BaseAccountHost}/api/Transaction/CompleteTransaction";
		public string GETStockRoute(string stockId)
			=> $"{BaseAccountHost}/api/Stock/GetStock/{stockId}";
	}
}