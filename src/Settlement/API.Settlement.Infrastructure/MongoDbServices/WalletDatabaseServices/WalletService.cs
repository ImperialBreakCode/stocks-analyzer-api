using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using Newtonsoft.Json.Linq;

namespace API.Settlement.Infrastructure.MongoDbServices.WalletDatabaseServices
{
	public class WalletService : IWalletService
	{
		private readonly IWalletRepository _walletRepository;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IConstantsHelperWrapper _infrastructureConstants;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEmailService _emailService;
		private readonly ITransactionUnitOfWork _transactionUnitOfWork;
		private readonly IOutboxUnitOfWork _outboxUnitOfWork;
		public WalletService(IWalletRepository walletRepository,
							 IMapperManagementWrapper mapperManagementWrapper,
							 IConstantsHelperWrapper infrastructureConstants,
							 IHttpClientFactory httpClientFactory,
							 IEmailService emailService,
							 ITransactionUnitOfWork transactionUnitOfWork,
							 IOutboxUnitOfWork outboxUnitOfWork)
		{
			_walletRepository = walletRepository;
			_mapperManagementWrapper = mapperManagementWrapper;
			_infrastructureConstants = infrastructureConstants;
			_httpClientFactory = httpClientFactory;
			_emailService = emailService;
			_transactionUnitOfWork = transactionUnitOfWork;
			_outboxUnitOfWork = outboxUnitOfWork;
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
		public async Task CheckCapital()
		{
			var wallets = _walletRepository.GetWallets();
			foreach (var wallet in wallets)
			{
				foreach (var stock in wallet.Stocks)
				{
					//var actualSingleStockPrice = 850; //TODO: hardcoded for testing
					var actualSingleStockPrice = await GetActualSingleStockPrice(stock.StockName);
					decimal actualTotalStockPrice = stock.Quantity * actualSingleStockPrice;
					double percentageDifference = (double)((actualTotalStockPrice - stock.InvestedAmount) / stock.InvestedAmount * 100);

					if (percentageDifference > 0)
					{
						if (percentageDifference >= 20)
						{
							var generatedTransaction = await PerformCapitalLossSale(wallet, stock, actualTotalStockPrice);

							await SendStockAlertEmail(percentageDifference, generatedTransaction);
						}
						else
						{
							await SendNotifyingEmail(wallet, percentageDifference);
						}

					}
					else if (percentageDifference < 0)
					{
						if (percentageDifference <= -15)
						{
							var generatedTransaction = await PerformCapitalLossSale(wallet, stock, actualTotalStockPrice);

							await SendStockAlertEmail(percentageDifference, generatedTransaction);
						}
						else
						{
							await SendNotifyingEmail(wallet, percentageDifference);
						}
					}
					else
					{
						continue;
					}
				}
			}
		}
		public void DeleteWallet(string walletId)
		{
			if (_walletRepository.ContainsWallet(walletId))
			{
				_walletRepository.DeleteWallet(walletId);
			}

		}

		private async Task SendNotifyingEmail(Wallet wallet, double percentageDifference)
		{
			var formattedPercentageDifference = percentageDifference != 0 ? percentageDifference < 0 ? $"-{Math.Abs(percentageDifference)}" : $"+{percentageDifference}" : "0";
			var emailDTO = _mapperManagementWrapper.NotifyingEmailMapper.CreateNotifyingEmailDTO(wallet.UserEmail, "Stock Alert", $"Your stock`s price has changed by {formattedPercentageDifference}%!");
			await _emailService.SendEmailWithoutAttachment(emailDTO);
		}

		private async Task SendStockAlertEmail(double percentageDifference, Transaction transaction)
		{
			var finalizeTransactionResponseDTO = _mapperManagementWrapper.FinalizeTransactionResponseDTOMapper.MapToFinalizeTransactionResponseDTO(transaction);
			var formattedPercentageDifference = percentageDifference != 0 ? percentageDifference < 0 ? $"-{Math.Abs(percentageDifference)}" : $"+{percentageDifference}" : "0";
			var emailDTO = _mapperManagementWrapper.FinalizingEmailMapper.CreateTransactionSummaryEmailDTO(finalizeTransactionResponseDTO, $"Your stock`s price has changed by {formattedPercentageDifference}%! It has been automatically sold!");
			await _emailService.SendEmailWithAttachment(emailDTO);
		}

		private async Task<Transaction> PerformCapitalLossSale(Wallet wallet, Stock stock, decimal actualTotalStockPrice)
		{
			_walletRepository.RemoveStock(wallet.WalletId, stock.StockId);

			var transaction = _mapperManagementWrapper.TransactionMapper.MapToSelllTransactionEntity(wallet, stock, actualTotalStockPrice);
			transaction.Message = _infrastructureConstants.MessageConstants.TransactionSuccessMessage;
			_transactionUnitOfWork.SuccessfulTransactions.Add(transaction);

			var outboxPendingMessageEntity = _mapperManagementWrapper.OutboxPendingMessageMapper.MapToOutboxPendingMessageEntity(transaction);
			_outboxUnitOfWork.PendingMessageRepository.AddPendingMessage(outboxPendingMessageEntity);

			return transaction;
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
				var response = await _httpClient.GetAsync(_infrastructureConstants.RouteConstants.GETStockPriceRoute(stockName));
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var stockData = JObject.Parse(json);
					price = (decimal)stockData["close"];
				}
			}
			return price;
		}


	}

}