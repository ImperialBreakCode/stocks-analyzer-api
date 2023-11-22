using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class TransactionWrapper : ITransactionWrapper
	{
		public IBuyService BuyService { get; }

		public ISellService SellService { get; }

		public TransactionWrapper(IBuyService buyService,
								ISellService sellService)
		{
			BuyService = buyService;
			SellService = sellService;
		}

		public async Task<AvailabilityResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			if (finalizeTransactionRequestDTO.IsSale)
			{
				return await SellService.SellStocks(finalizeTransactionRequestDTO);
			}

			return await BuyService.BuyStocks(finalizeTransactionRequestDTO);
		}
	}
}