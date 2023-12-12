using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.HangfireInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.TransactionCompletionInterfaces;

namespace API.Settlement.Application.Services.HangfireServices
{
	public class HangfireJobService : IHangfireJobService
	{
		private readonly IWalletService _walletService;
		private readonly IRabbitMQService _rabbitMQService;
		private readonly ITransactionCompletionService _transactionCompletionService;
		private readonly IFailedTransactionCompletionService _failedTransactionCompletionService;

		public HangfireJobService(IWalletService walletService,
								  IRabbitMQService rabbitMQService,
								  ITransactionCompletionService transactionCompletionService,
								  IFailedTransactionCompletionService failedTransactionCompletionService)
		{
			_walletService = walletService;
			_rabbitMQService = rabbitMQService;
			_transactionCompletionService = transactionCompletionService;
			_failedTransactionCompletionService = failedTransactionCompletionService;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			await _transactionCompletionService.FinalizeTransaction(availabilityResponseDTO);
		}

		public async Task RecurringFailedTransactionsJob()
		{
			await _failedTransactionCompletionService.ProcessFailedTransactions();
		}

		public async Task RecurringCapitalCheckJob()
		{
			await _walletService.CheckCapital();
		}

		public void RecurringRabbitMQMessageSenderJob()
		{
			_rabbitMQService.PerformMessageSending();
		}

	}
}