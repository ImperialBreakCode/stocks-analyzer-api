using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces;

namespace API.Settlement.Application.Services.TransactionServices.OrderProcessingServices
{
	public class TransactionProcessingService : ITransactionProcessingService
	{
		public IBuyService BuyService { get; }
		public ISellService SellService { get; }

		public TransactionProcessingService(IBuyService buyService,
								  ISellService sellService)
		{
			BuyService = buyService;
			SellService = sellService;
		}

		public async Task<AvailabilityResponseDTO> ProcessTransactions(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var transactionType = finalizeTransactionRequestDTO.IsSale ? TransactionType.Sell : TransactionType.Buy;

			if (transactionType == TransactionType.Buy)
			{
				return await BuyService.BuyStocks(finalizeTransactionRequestDTO);
			}

			return await SellService.SellStocks(finalizeTransactionRequestDTO);
		}

	}
}