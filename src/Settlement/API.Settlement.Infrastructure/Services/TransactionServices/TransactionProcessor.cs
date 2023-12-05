using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces;
using API.Settlement.Infrastructure.Services.SQLiteServices;
using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.TransactionServices
{
	public class TransactionProcessor : ITransactionProcessor
	{
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionResponseHandlerService _transactionResponseHandlerService;
		private readonly IWalletService _walletService;
		private readonly IEmailService _emailService;
		private readonly ITransactionDatabaseContext _transactionDatabaseContext;
		public TransactionProcessor(IHttpClientFactory httpClientFactory,
									IMapperManagementWrapper mapperManagementWrapper,
									IInfrastructureConstants infrastructureConstants,
									ITransactionResponseHandlerService transactionResponseHandlerService,
									IWalletService walletService,
									IEmailService emailService,
									ITransactionDatabaseContext transactionDatabaseContext)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_infrastructureConstants = infrastructureConstants;
			_transactionResponseHandlerService = transactionResponseHandlerService;
			_walletService = walletService;
			_emailService = emailService;
			_transactionDatabaseContext = transactionDatabaseContext;
		}

		public async Task FinalizeTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var finalizeTransactionResponseDTO = _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTO(availabilityResponseDTO);
			HttpResponseMessage response = await SendTransactionRequest(finalizeTransactionResponseDTO);
			if (response != null && response.IsSuccessStatusCode)
			{
				await SendTransactionSummaryEmail(finalizeTransactionResponseDTO);
				await UpdateStocksInWallet(finalizeTransactionResponseDTO);
			}

			await HandleTransactionResponse(response, finalizeTransactionResponseDTO);
		}
		public async Task ProcessFailedTransactions()
		{
			var failedTransactionEntities = _transactionDatabaseContext.FailedTransactions.GetAll();
			var finalizeTransactionResponseDTOs = _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTOs(failedTransactionEntities);
			foreach ( var finalizeTransactionResponseDTO in finalizeTransactionResponseDTOs)
			{
				HttpResponseMessage response = await SendTransactionRequest(finalizeTransactionResponseDTO);
				response = new HttpResponseMessage(HttpStatusCode.OK);
				if (response != null && response.IsSuccessStatusCode)
				{
					await SendTransactionSummaryEmail(finalizeTransactionResponseDTO);
					await UpdateStocksInWallet(finalizeTransactionResponseDTO);
				}

				await HandleTransactionResponse(response, finalizeTransactionResponseDTO);
			}

		}


		private async Task<HttpResponseMessage> SendTransactionRequest(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			HttpResponseMessage response = null;
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				try
				{
					response = await httpClient.PostAsync(_infrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
					//response = new HttpResponseMessage(HttpStatusCode.BadRequest);
				}
				catch (Exception ex)
				{
					response = new HttpResponseMessage(HttpStatusCode.BadGateway);
				}
				return response;
			}
		}

		private async Task SendTransactionSummaryEmail(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			var emailDTO = _mapperManagementWrapper.FinalizingEmailMapper.CreateTransactionSummaryEmailDTO(finalizeTransactionResponseDTO, "Transaction Summary Report");
			await _emailService.SendEmailWithAttachment(emailDTO);
		}
		private async Task UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			_walletService.UpdateStocksInWallet(finalizeTransactionResponseDTO);
		}
		private async Task HandleTransactionResponse(HttpResponseMessage response, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			var transactions = _mapperManagementWrapper.TransactionMapper.MapToTransactionEntities(finalizeTransactionResponseDTO);
			_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
		}
	}
}
