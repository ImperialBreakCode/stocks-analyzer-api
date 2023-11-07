using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Helpers.Constants
{
	public class InfrastructureConstants : IInfrastructureConstants
	{
		private static readonly string _transactionDeclinedMessage = "Transaction declined: Insufficient funds in your account.";
		private static readonly string _transactionScheduledMessage = "Transaction scheduled for execution tomorrow at 00:01:00.";
		private static readonly string _transactionSuccessMessage = "Transaction completed successfully.";
		private static readonly decimal _commission = 0.0005M;
		public string TransactionDeclinedMessage { get { return _transactionDeclinedMessage; } }

		public string TransactionScheduledMessage { get { return _transactionScheduledMessage; } }

		public string TransactionSuccessMessage { get { return _transactionSuccessMessage; } }

		public decimal Commission { get { return _commission; } }

		public string GETWalletBalanceRoute(string walletId) { return $"/api/Wallet/GetWalletBalance/{walletId}"; }
		public string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO) { return "/api/Transaction/CompleteTransaction"; }
		public string GETStockRoute(string stockId) { return $"/api/Stock/GetStock/{stockId}"; }
	}
}