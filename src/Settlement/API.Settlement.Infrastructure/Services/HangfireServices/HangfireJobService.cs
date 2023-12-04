using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
	public class HangfireJobService : IHangfireJobService
	{
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionResponseHandlerService _transactionResponseHandlerService;
		private readonly ITransactionDatabaseContext _transactionDatabaseContext;
		private readonly IWalletService _walletService;
		private readonly IEmailService _emailService;
		private readonly IRabbitMQService _rabbitMQService;


		public HangfireJobService(IHttpClientFactory httpClientFactory,
						IMapperManagementWrapper mapperManagementWrapper,
						IInfrastructureConstants infrastructureConstants,
						ITransactionResponseHandlerService transactionResponseHandlerService,
						ITransactionDatabaseContext unitOfWork,
						IWalletService walletService,
						IEmailService emailService,
						IRabbitMQService rabbitMQService)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_infrastructureConstants = infrastructureConstants;
			_transactionResponseHandlerService = transactionResponseHandlerService;
			_transactionDatabaseContext = unitOfWork;
			_walletService = walletService;
			_emailService = emailService;
			_rabbitMQService = rabbitMQService;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			HttpResponseMessage response = null;
			var finalizeTransactionResponseDTO = _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTO(availabilityResponseDTO);

			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				try
				{
					response = new HttpResponseMessage(HttpStatusCode.BadRequest);
					//response = await httpClient.PostAsync(_infrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
				}
				catch (Exception ex)
				{
                    await Console.Out.WriteLineAsync(ex.Message);
                }
				finally
				{
					var emailDTO = _mapperManagementWrapper.FinalizingEmailMapper.CreateTransactionSummaryEmailDTO(finalizeTransactionResponseDTO, "Transaction Summary Report");
					await _emailService.SendEmailWithAttachment(emailDTO);

					_walletService.UpdateStocksInWallet(finalizeTransactionResponseDTO);

					var transactions = _mapperManagementWrapper.TransactionMapper.MapToTransactionEntities(finalizeTransactionResponseDTO);
					_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
				}
			}
		}

		public async Task RecurringFailedTransactionsJob()
		{
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var failedTransactionEntities = _transactionDatabaseContext.FailedTransactions.GetAll();
				var finalizeTransactionResponseDTOs = _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTOs(failedTransactionEntities);
				foreach (var finalizeTransactionResponseDTO in finalizeTransactionResponseDTOs)
				{
					var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response = null;
					try
					{
						response = new HttpResponseMessage(HttpStatusCode.OK);
						//response = await httpClient.PostAsync(_infrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
					}
					catch (Exception ex)
					{
						await Console.Out.WriteLineAsync(ex.Message);
					}
					finally
					{
						var transactions = _mapperManagementWrapper.TransactionMapper.MapToTransactionEntities(finalizeTransactionResponseDTO);
						_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
					}
					

				}
			}
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