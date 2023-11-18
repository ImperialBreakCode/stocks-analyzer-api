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

		private void PerformBuyLogic(Wallet existingWallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				Stock? existingStock = _walletRepository.GetStockFromWallet(existingWallet, stockInfoResponseDTO.StockId);
				if (existingStock == null)
				{
					existingStock = _transactionMapperService.MapToStockEntity(stockInfoResponseDTO);
				}
				else
				{
					existingStock.InvestedAmount += CalculateBuyPriceWithoutCommission(stockInfoResponseDTO.TotalPriceIncludingCommission);
					existingStock.AverageSingleStockPrice = CalculateBuyPriceWithoutCommission((existingStock.AverageSingleStockPrice + stockInfoResponseDTO.SinglePriceIncludingCommission) / 2);
					existingStock.Quantity += stockInfoResponseDTO.Quantity;
				}
				_walletRepository.AddStock(existingWallet.WalletId, existingStock);

			}
		}
		private void PerformSaleLogic(Wallet existingWallet, FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				Stock? existingStock = _walletRepository.GetStockFromWallet(existingWallet, stockInfoResponseDTO.StockId);
				if (existingStock != null && existingStock.Quantity >= stockInfoResponseDTO.Quantity)
				{
					existingStock.InvestedAmountPerStock = existingStock.InvestedAmount / existingStock.Quantity;
					existingStock.Quantity -= stockInfoResponseDTO.Quantity;
					existingStock.InvestedAmount = existingStock.InvestedAmountPerStock * existingStock.Quantity;

				}
				
				_walletRepository.UpdateStock(existingWallet.WalletId, existingStock);

			}
		}


		private decimal CalculateBuyPriceWithoutCommission(decimal priceIncludingCommission) => priceIncludingCommission - (priceIncludingCommission * _infrastructureConstants.Commission);
	}
}
