using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces;
using API.Settlement.Infrastructure.Services.TransactionServices;
using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
	public class HangfireJobService : IHangfireJobService
	{
		private readonly IWalletService _walletService;
		private readonly IRabbitMQService _rabbitMQService;
		private readonly ITransactionProcessor _transactionProcessor;

		public HangfireJobService(IWalletService walletService,
								  IRabbitMQService rabbitMQService,
								  ITransactionProcessor transactionProcessor)
		{
			_walletService = walletService;
			_rabbitMQService = rabbitMQService;
			_transactionProcessor = transactionProcessor;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			await _transactionProcessor.FinalizeTransaction(availabilityResponseDTO);
		}

		public async Task RecurringFailedTransactionsJob()
		{
			await _transactionProcessor.ProcessFailedTransactions();
		}

		public async Task RecurringCapitalCheckJob()
		{
			await _walletService.CapitalCheck();
		}

		public async Task RecurringRabbitMQMessageSenderJob()
		{
			_rabbitMQService.PerformMessageSending();
		}

	}
}