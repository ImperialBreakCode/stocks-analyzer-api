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

		public async Task<FinalizeTransactionResponseDTO> ProcessNextDayAccountTransaction(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			if (finalizeTransactionRequestDTO.IsSale)
			{
				return await SellService.SellStocks(finalizeTransactionRequestDTO);
			}
			
			return await BuyService.BuyStocks(finalizeTransactionRequestDTO);
		}
	}
}
