﻿using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Enums;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Domain.Interfaces
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
		decimal GetCommissionBasedOnUserType(UserType userRank);
		string GetMessageBasedOnStatus(Status status);

		bool IsInitializedRecurringFailedTransactionsJob { get; set; }
		bool IsInitializedRecurringCapitalLossCheckJob {get;set;}
	}
}