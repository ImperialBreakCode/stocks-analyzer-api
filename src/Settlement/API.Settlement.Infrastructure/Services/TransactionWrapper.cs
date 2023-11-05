using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services
{
	public class TransactionWrapper : ITransactionWrapper
	{
		public IBuyService BuyService { get; }

		public ISellService SellService { get; }

		public TransactionWrapper(IBuyService buyService, ISellService sellService)
		{
			BuyService = buyService;
			SellService = sellService;
		}

		public async Task<IEnumerable<FinalizeTransactionResponseDTO>> ProcessNextDayAccountTransactions(IEnumerable<FinalizeTransactionRequestDTO> finalizeTransactionRequestDTOs)
		{
			var finalizeTransactionBuyRequestDTOs = finalizeTransactionRequestDTOs.Where(x => !x.IsSale);
			var finalizeTransactionSellRequestDTOs = finalizeTransactionRequestDTOs.Where(x => x.IsSale);


			var finalizeTransactionBuyResponseDTOs = await BuyService.BuyStocks(finalizeTransactionBuyRequestDTOs);
			var finalizeTransactionSellResponseDTOs = await SellService.SellStocks(finalizeTransactionSellRequestDTOs);

			var finalizeTransactionResponseDTOs = finalizeTransactionBuyResponseDTOs.Concat(finalizeTransactionSellResponseDTOs);

			return finalizeTransactionResponseDTOs;
		}
	}
}
