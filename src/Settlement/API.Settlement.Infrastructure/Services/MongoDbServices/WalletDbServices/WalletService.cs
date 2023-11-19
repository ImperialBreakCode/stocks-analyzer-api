using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
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
		public WalletService(IWalletRepository walletRepository,
							ITransactionMapperService transactionMapperService,
							IInfrastructureConstants infrastructureConstants)
		{
			_walletRepository = walletRepository;
			_transactionMapperService = transactionMapperService;
			_infrastructureConstants = infrastructureConstants;
		}

		public void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			Wallet? existingWallet = _walletRepository.GetWalletById(finalizeTransactionResponseDTO.WalletId);
			if (existingWallet == null)
			{
				var newWallet = _transactionMapperService.MapToWalletEntity(finalizeTransactionResponseDTO);
				_walletRepository.CreateWallet(newWallet);
				existingWallet = _walletRepository.GetWalletById(finalizeTransactionResponseDTO.WalletId);
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
		private void PerformBuyLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				Stock? stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if (stock == null)
				{
					var newStock = new Stock()
					{
						StockId = stockInfoResponseDTO.StockId,
						StockName = stockInfoResponseDTO.StockName,
						Quantity = stockInfoResponseDTO.Quantity,
						AverageSingleStockPrice = CalculateBuyPriceWithoutCommission(stockInfoResponseDTO.SinglePriceIncludingCommission),
						InvestedAmount = CalculateBuyPriceWithoutCommission(stockInfoResponseDTO.TotalPriceIncludingCommission)
					};
					_walletRepository.AddStock(wallet.WalletId, newStock);
				}
				else
				{
					stock.Quantity += stockInfoResponseDTO.Quantity;
					stock.InvestedAmount += CalculateBuyPriceWithoutCommission(stockInfoResponseDTO.TotalPriceIncludingCommission);
					stock.AverageSingleStockPrice = (stock.AverageSingleStockPrice + (CalculateBuyPriceWithoutCommission(stockInfoResponseDTO.SinglePriceIncludingCommission))) / 2;
					_walletRepository.UpdateStock(wallet.WalletId, stock);
				}
			}
		}

		private void PerformSaleLogic(Wallet wallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				var stock = _walletRepository.GetStockFromWallet(wallet.WalletId, stockInfoResponseDTO.StockId);
				if(stock != null)
				{
					if (stock != null && stock.Quantity >= stockInfoResponseDTO.Quantity)
					{
						stock.Quantity -= stockInfoResponseDTO.Quantity;
						stock.InvestedAmount = stock.InvestedAmount - (CalculateSalePriceWithoutCommission(stock.AverageSingleStockPrice) * stockInfoResponseDTO.Quantity);
						stock.AverageSingleStockPrice = (CalculateSalePriceWithoutCommission(stock.AverageSingleStockPrice) + CalculateSalePriceWithoutCommission(stockInfoResponseDTO.SinglePriceIncludingCommission)) / 2;
					}
					if (stock.Quantity == 0) { _walletRepository.RemoveStock(wallet.WalletId, stock.StockId); }
					else { _walletRepository.UpdateStock(wallet.WalletId, stock); }
				}
				

			}
		}
		private decimal CalculateBuyPriceWithoutCommission(decimal priceIncludingCommission) => priceIncludingCommission / (1 + _infrastructureConstants.Commission);
		private decimal CalculateSalePriceWithoutCommission(decimal priceIncludingCommission) => priceIncludingCommission / (1 - _infrastructureConstants.Commission);
	}

}