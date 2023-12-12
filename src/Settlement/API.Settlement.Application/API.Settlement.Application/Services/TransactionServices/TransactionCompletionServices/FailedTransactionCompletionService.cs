using API.Settlement.Application.Mappings;
using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.HTTPInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.TransactionCompletionInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.TransactionServices.TransactionCompletionServices
{
	public class FailedTransactionCompletionService : IFailedTransactionCompletionService
	{
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConstantsHelperWrapper _infrastructureConstants;
		private readonly ITransactionResponseHandlerService _transactionResponseHandlerService;
		private readonly IWalletService _walletService;
		private readonly IEmailService _emailService;
		private readonly ITransactionUnitOfWork _transactionUnitOfWork;
		public FailedTransactionCompletionService(IHttpClientFactory httpClientFactory,
									IMapperManagementWrapper mapperManagementWrapper,
									IConstantsHelperWrapper infrastructureConstants,
									ITransactionResponseHandlerService transactionResponseHandlerService,
									IWalletService walletService,
									IEmailService emailService,
									ITransactionUnitOfWork transactionUnitOfWork)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_infrastructureConstants = infrastructureConstants;
			_transactionResponseHandlerService = transactionResponseHandlerService;
			_walletService = walletService;
			_emailService = emailService;
			_transactionUnitOfWork = transactionUnitOfWork;
		}
		public async Task ProcessFailedTransactions()
		{
			var failedTransactionEntities = GetAllFailedTransactions();
			var finalizeTransactionResponseDTOs = MapFailedTransactionsToFinalizeDTOs(failedTransactionEntities);
			foreach (var finalizeTransactionResponseDTO in finalizeTransactionResponseDTOs)
			{
				//var response = await SendFinalizingTransactionRequest(finalizeTransactionResponseDTO);
				var response = new HttpResponseMessage(HttpStatusCode.OK);
				if (response != null && response.IsSuccessStatusCode)
				{
					var updatedFinalizeTransactionResponseDTO = UpdateStockInfoMessageToSuccessful(finalizeTransactionResponseDTO);
					await SendTransactionSummaryEmail(updatedFinalizeTransactionResponseDTO);
					UpdateStocksInWallet(finalizeTransactionResponseDTO);
				}
				HandleTransactionResponse(response, finalizeTransactionResponseDTO);
			}
		}

		private FinalizeTransactionResponseDTO UpdateStockInfoMessageToSuccessful(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			return _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.UpdateStockInfoMessageToSuccessful(finalizeTransactionResponseDTO);
		}

		private IEnumerable<FinalizeTransactionResponseDTO> MapFailedTransactionsToFinalizeDTOs(IEnumerable<Transaction> failedTransactionEntities)
		{
			return _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTOs(failedTransactionEntities);
		}

		private IEnumerable<Transaction> GetAllFailedTransactions()
		{
			return _transactionUnitOfWork.FailedTransactions.GetAll();
		}
		 
		private async Task<HttpResponseMessage> SendFinalizingTransactionRequest(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			HttpResponseMessage response = null;
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				try
				{
					response = await httpClient.PostAsync(_infrastructureConstants.RouteConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
				}
				catch (Exception ex)
				{
					response = new HttpResponseMessage(HttpStatusCode.BadGateway);
				}
			}
			return response;
		}
		private async Task SendTransactionSummaryEmail(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			var emailDTO = _mapperManagementWrapper.FinalizingEmailMapper.CreateTransactionSummaryEmailDTO(finalizeTransactionResponseDTO, "Transaction Summary Report");
			await _emailService.SendEmailWithAttachment(emailDTO);
		}
		private void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			_walletService.UpdateStocksInWallet(finalizeTransactionResponseDTO);
		}
		private void HandleTransactionResponse(HttpResponseMessage response, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			var transactions = _mapperManagementWrapper.TransactionMapper.MapToTransactionEntities(finalizeTransactionResponseDTO);
			_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
		}

	}
}