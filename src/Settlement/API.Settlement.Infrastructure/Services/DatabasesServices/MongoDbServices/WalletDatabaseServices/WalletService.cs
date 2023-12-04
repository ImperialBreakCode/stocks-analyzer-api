using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MongoDbServices.WalletDatabasebServices
{
    public class WalletService : IWalletService
	{
		private readonly IWalletRepository _walletRepository;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEmailService _emailService;
		private readonly ITransactionDatabaseContext _transactionDatabaseContext;
		private readonly IRabbitMQProducer _rabbitMQSellTransactionProducer;
		private readonly IOutboxDatabaseContext _outboxDatabaseContext;
		public WalletService(IWalletRepository walletRepository,
							IMapperManagementWrapper transactionMapperService,
							IInfrastructureConstants infrastructureConstants,
							IHttpClientFactory httpClientFactory,
							IEmailService emailService,
							ITransactionDatabaseContext transactionDatabaseContext,
							IRabbitMQProducer rabbitMQSellTransactionProducer,
							IOutboxDatabaseContext outboxDatabaseContext)
		{
			_walletRepository = walletRepository;
			_mapperManagementWrapper = transactionMapperService;
			_infrastructureConstants = infrastructureConstants;
			_httpClientFactory = httpClientFactory;
			_emailService = emailService;
			_transactionDatabaseContext = transactionDatabaseContext;
			_rabbitMQSellTransactionProducer = rabbitMQSellTransactionProducer;
			_outboxDatabaseContext = outboxDatabaseContext;
		}

		public void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			Wallet? existingWallet = _walletRepository.GetWalletById(finalizeTransactionResponseDTO.WalletId);
			if (existingWallet == null)
			{
				var newWallet = _mapperManagementWrapper.WalletMapper.MapToWalletEntity(finalizeTransactionResponseDTO);
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
					var actualSingleStockPrice = 170;
					//var actualSingleStockPrice = await GetActualSingleStockPrice(stock.StockName);
					decimal actualTotalStockPrice = stock.Quantity * actualSingleStockPrice;
					double percentageDifference = (double)((actualTotalStockPrice - stock.InvestedAmount) / stock.InvestedAmount * 100);

					if (percentageDifference > 0)
					{
						var emailDTO = _mapperManagementWrapper.NotifyingEmailMapper.CreateNotifyingEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has increased by {percentageDifference}%!");
						await _emailService.SendEmailWithoutAttachment(emailDTO);
					}
					else if (percentageDifference < 0)
					{
						if (percentageDifference <= -15)
						{
							_walletRepository.RemoveStock(wallet.WalletId, stock.StockId);
							var transaction = _mapperManagementWrapper.TransactionMapper.MapToSelllTransactionEntity(wallet, stock, actualTotalStockPrice);
							transaction.Message = _infrastructureConstants.TransactionSuccessMessage;
							_transactionDatabaseContext.SuccessfulTransactions.Add(transaction);
							var outboxPendingMessageEntity = _mapperManagementWrapper.OutboxPendingMessageMapper.MapToOutboxPendingMessageEntity(transaction);
							_outboxDatabaseContext.PendingMessageRepository.AddPendingMessage(outboxPendingMessageEntity);

							var emailDTO = _mapperManagementWrapper.NotifyingEmailMapper.CreateNotifyingEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has decreased by {percentageDifference}%! It has been automatically sold!");
							await _emailService.SendEmailWithoutAttachment(emailDTO);
						}
						else
						{
							var emailDTO = _mapperManagementWrapper.NotifyingEmailMapper.CreateNotifyingEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock price has decreased by {percentageDifference}%!");
							await _emailService.SendEmailWithoutAttachment(emailDTO);
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
					stock = _mapperManagementWrapper.StockMapper.MapToStockEntity(stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);
					
					_walletRepository.AddStock(wallet.WalletId, stock);
				}
				else
				{
					stock = _mapperManagementWrapper.StockMapper.UpdateStockForPurchase(stock, stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);

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
						stock = _mapperManagementWrapper.StockMapper.UpdateStockForSale(stock, stockInfoResponseDTO, finalizeTransactionResponseDTO.UserRank);
						
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