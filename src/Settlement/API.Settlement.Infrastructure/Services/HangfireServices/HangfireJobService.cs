﻿using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
    public class HangfireJobService : IHangfireJobService
	{
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionResponseHandlerService _transactionResponseHandlerService;
		private readonly ITransactionDatabaseContext _unitOfWork;
		private readonly IWalletService _walletService;


		public HangfireJobService(IHttpClientFactory httpClientFactory,
						ITransactionMapperService transactionMapperService,
						IInfrastructureConstants infrastructureConstants,
						ITransactionResponseHandlerService transactionResponseHandlerService,
						ITransactionDatabaseContext unitOfWork,
						IWalletService walletService)
		{
			_httpClientFactory = httpClientFactory;
			_transactionMapperService = transactionMapperService;
			_infrastructureConstants = infrastructureConstants;
			_transactionResponseHandlerService = transactionResponseHandlerService;
			_unitOfWork = unitOfWork;
			_walletService = walletService;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
			var finalizeTransactionResponseDTO = _transactionMapperService.MapToFinalizeTransactionResponseDTO(availabilityResponseDTO);
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				try
				{
					response = await httpClient.PostAsync(_infrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
				}
				catch(Exception ex)
				{
                    await Console.Out.WriteLineAsync(ex.Message);
                }
				finally
				{
					_walletService.UpdateStocksInWallet(finalizeTransactionResponseDTO);

					var transactions = _transactionMapperService.MapToTransactionEntities(finalizeTransactionResponseDTO);
					_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
				}
			}
		}

		public async Task RecurringFailedTransactionsJob()
		{
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var failedTransactionEntities = _unitOfWork.FailedTransactions.GetAll();
				var finalizeTransactionResponseDTOs = _transactionMapperService.MapToFinalizeTransactionResponseDTOs(failedTransactionEntities);
				foreach (var finalizeTransactionResponseDTO in finalizeTransactionResponseDTOs)
				{
					var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
					try
					{
						//response = await httpClient.PostAsync(_infrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
					}
					catch (Exception ex)
					{
						await Console.Out.WriteLineAsync(ex.Message);
					}
					finally
					{
						var transactions = _transactionMapperService.MapToTransactionEntities(finalizeTransactionResponseDTO);
						_transactionResponseHandlerService.HandleTransactionResponse(response, transactions);
					}
					

				}
			}
		}
		public async Task RecurringCapitalLossCheckJob()
		{
			await _walletService.CapitalLossCheck();
		}

	}
}