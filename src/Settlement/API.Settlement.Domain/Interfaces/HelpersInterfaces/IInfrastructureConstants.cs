using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface IInfrastructureConstants
	{
		string TransactionDeclinedMessage { get; }
		string TransactionScheduledMessage { get; }
		string TransactionSuccessMessage { get; }
		string TransactionConnectionIssueMessage { get; }
		string GETWalletBalanceRoute(string walletId);
		string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
		string GETStockRoute(string stockId);
		string GETStockPriceRoute(string stockName);
		decimal GetCommissionBasedOnUserType(UserRank userRank);
		string GetMessageBasedOnStatus(Status status);
		bool IsInitializedRecurringFailedTransactionsJob { get; set; }
		bool IsInitializedRecurringCapitalLossCheckJob { get; set; }
		bool IsInitializedRecurringRabbitMQMessageSenderJob { get; set; }
	}
}