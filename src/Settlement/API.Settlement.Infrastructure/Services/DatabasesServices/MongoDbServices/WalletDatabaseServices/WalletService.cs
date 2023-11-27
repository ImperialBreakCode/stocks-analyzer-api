﻿using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MongoDbServices.WalletDbServices
{
    public class WalletService : IWalletService
	{
		private readonly IWalletRepository _walletRepository;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEmailService _emailService;
		private readonly ITransactionDatabaseContext _transactionDatabaseContext;
		public WalletService(IWalletRepository walletRepository,
							ITransactionMapperService transactionMapperService,
							IInfrastructureConstants infrastructureConstants,
							IHttpClientFactory httpClientFactory,
							IEmailService emailService,
							ITransactionDatabaseContext transactionDatabaseContext)
		{
			_walletRepository = walletRepository;
			_transactionMapperService = transactionMapperService;
			_infrastructureConstants = infrastructureConstants;
			_httpClientFactory = httpClientFactory;
			_emailService = emailService;
			_transactionDatabaseContext = transactionDatabaseContext;
		}

		public void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			Wallet? existingWallet = _walletRepository.GetWalletById(finalizeTransactionResponseDTO.WalletId);
			if (existingWallet == null)
			{
				var newWallet = _transactionMapperService.MapToWalletEntity(finalizeTransactionResponseDTO);
				_walletRepository.CreateWallet(newWallet);
				existingWallet = newWallet;
			}
			if (finalizeTransactionResponseDTO.IsSale)
			{
				PerformSaleLogic(existingWallet, finalizeTransactionResponseDTO);
			}
			else
			{
				PerformBuyLogic(existingWallet, finalizeTransactionResponseDTO);
			}

		}
		public async Task CapitalLossCheck()
		{
			var wallets = _walletRepository.GetWallets();
			foreach (var wallet in wallets)
			{
				foreach (var stock in wallet.Stocks)
				{
					var actualSingleStockPrice = 900;//await GetActualSingleStockPrice(stock.StockName);
					var actualTotalStockPrice = stock.Quantity * actualSingleStockPrice;
					var percentageDifference = (actualTotalStockPrice - stock.InvestedAmount) / stock.InvestedAmount * 100;

					if (percentageDifference > 0)
					{
						var emailDTO = _transactionMapperService.CreateEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has increased by {percentageDifference}%!");
						await _emailService.SendEmail(emailDTO);
					}
					else if (percentageDifference < 0)
					{
						if (percentageDifference <= -15)
						{
							_walletRepository.RemoveStock(wallet.WalletId, stock.StockId);
							//TODO: 
							//var transactionDTO = _transactionMapperService.MapToTransactionDTO(wallet, stock);
							//give it to the outbox pattern table failed
							//outbox pattern give it to the rabbidmq;
							//remove the row from the failed table in OP and add to the Succes table in OP
							//_transactionDatabaseContext.SuccessfulTransactions.Add(transactionDTO);

							var emailDTO = _transactionMapperService.CreateEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has decreased by {percentageDifference}%! It has been automatically sold!");
							await _emailService.SendEmail(emailDTO);
						}
						else
						{
							var emailDTO = _transactionMapperService.CreateEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has decreased by {percentageDifference}%!");
							await _emailService.SendEmail(emailDTO);
						}
					}
					else
					{
						continue;
					}
				}
			}
		}

		private void PerformBuyLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				Stock? stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if (stock == null)
				{
					stock = _transactionMapperService.MapToStockEntity(stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);
					
					_walletRepository.AddStock(wallet.WalletId, stock);
				}
				else
				{
					stock = _transactionMapperService.UpdateStockForPurchase(stock, stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);

					_walletRepository.UpdateStock(wallet.WalletId, stock);
				}
			}
		}

		private void PerformSaleLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				var stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if (stock != null)
				{
					if (stock != null && stock.Quantity >= stockInfoResponseDTO.Quantity)
					{
						stock = _transactionMapperService.UpdateStockForSale(stock, stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);
						
					}
					if (stock.Quantity == 0) { _walletRepository.RemoveStock(wallet.WalletId, stock.StockId); }
					else { _walletRepository.UpdateStock(wallet.WalletId, stock); }
				}


			}
		}
		private async Task<decimal> GetActualSingleStockPrice(string stockName)
		{
			decimal price = 0;
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				var response = await _httpClient.GetAsync(_infrastructureConstants.GETStockPriceRoute(stockName));
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var stockData = JObject.Parse(json);
					price = (decimal)stockData["Close"];
				}
			}
			return price;
		}

	}

}