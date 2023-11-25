﻿using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Infrastructure.Helpers.Constants
{
	public class InfrastructureConstants : IInfrastructureConstants
	{
		private static readonly string _transactionDeclinedMessage = "Transaction declined: Insufficient resources.";
		private static readonly string _transactionScheduledMessage = "Transaction scheduled for execution tomorrow at 00:01:00.";
		private static readonly string _transactionSuccessMessage = "Transaction completed successfully.";
		private static readonly string _transactionConnectionIssueMessage = "Transaction connection issue: Unable to process the transaction at the moment.";
		private static readonly decimal _baseCommission = 0.0005M;
		private static readonly decimal _specialTraderCommission = 0.0004M;
		private static readonly decimal _vipTraderCommission = 0.0003M;
		private static readonly string _baseAccountHost = "https://localhost:5032";
		private static readonly string _baseStockAPIHost = "https://localhost:";
		private static bool _isInitializedRecurringFailedTransactionsJob = false;
		private static bool _isInitializedRecurringCapitalLossCheckJob = false;
		public string TransactionDeclinedMessage => _transactionDeclinedMessage;

		public string TransactionScheduledMessage => _transactionScheduledMessage;

		public string TransactionSuccessMessage => _transactionSuccessMessage;
		public string TransactionConnectionIssueMessage => _transactionConnectionIssueMessage;
		public string BaseAccountHost => _baseAccountHost;
		public string BaseStockAPIHost => _baseStockAPIHost;

		public string GETWalletBalanceRoute(string walletId)
			=> $"{BaseAccountHost}/api/Wallet/GetWalletBalance/{walletId}";
		public string POSTCompleteTransactionRoute(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
			=> $"{BaseAccountHost}/api/Transaction/CompleteTransaction";
		public string GETStockRoute(string stockId)
			=> $"{BaseAccountHost}/api/Stock/GetStock/{stockId}";
		public string GETStockPriceRoute(string stockName)
			=> $"{BaseStockAPIHost}/api/StockAPI/Stock/Price/{stockName}";

		public decimal GetCommissionBasedOnUserType(UserType userRank)
		{
			switch (userRank)
			{
				case UserType.Demo:
				case UserType.RegularTrader: return _baseCommission;
				case UserType.SpecialTrader: return _specialTraderCommission;
				case UserType.VipTrader: return _vipTraderCommission;
				default: throw new Exception("Invalid user type!");
			}
		}

		public string GetMessageBasedOnStatus(Status status)
		{
			switch (status)
			{
				case Status.Declined: return _transactionDeclinedMessage;
				case Status.Success: return _transactionSuccessMessage;
				case Status.Scheduled: return _transactionScheduledMessage;
				default: return String.Empty;

			}
		}

		public bool IsInitializedRecurringFailedTransactionsJob
		{
			get
			{
				return _isInitializedRecurringFailedTransactionsJob;
			}
			set
			{
				_isInitializedRecurringFailedTransactionsJob = value;
			}
		}
		public bool IsInitializedRecurringCapitalLossCheckJob
		{
			get
			{
				return _isInitializedRecurringCapitalLossCheckJob;
			}
			set
			{
				_isInitializedRecurringCapitalLossCheckJob = value;
			}
		}

	}
}