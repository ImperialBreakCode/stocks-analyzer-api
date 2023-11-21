﻿using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
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
		public WalletService(IWalletRepository walletRepository,
							ITransactionMapperService transactionMapperService,
							IInfrastructureConstants infrastructureConstants,
							IHttpClientFactory httpClientFactory,
							IEmailService emailService)
		{
			_walletRepository = walletRepository;
			_transactionMapperService = transactionMapperService;
			_infrastructureConstants = infrastructureConstants;
			_httpClientFactory = httpClientFactory;
			_emailService = emailService;
		}

		public void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, UserType userRank)
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
				PerformSaleLogic(existingWallet, finalizeTransactionResponseDTO, userRank);
			}
			else
			{
				PerformBuyLogic(existingWallet, finalizeTransactionResponseDTO, userRank);
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

		private void PerformBuyLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, UserType userRank)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				Stock? stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if (stock == null)
				{
					stock = _transactionMapperService.MapToStockEntity(stockInfoResponseDTO, userRank); //finalizeTransactionResponseDTO.Rank;
					
					_walletRepository.AddStock(wallet.WalletId, stock);
				}
				else
				{
					stock = _transactionMapperService.UpdateStockForPurchase(stock, stockInfoResponseDTO, userRank); //finalizeTransactionResponseDTO.Rank;

					_walletRepository.UpdateStock(wallet.WalletId, stock);
				}
			}
		}

		private void PerformSaleLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, UserType userRank)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				var stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if (stock != null)
				{
					if (stock != null && stock.Quantity >= stockInfoResponseDTO.Quantity)
					{
						stock = _transactionMapperService.UpdateStockForSale(stock, stockInfoResponseDTO, userRank); //finalizeTransactionResponseDTO.Rank;
						
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