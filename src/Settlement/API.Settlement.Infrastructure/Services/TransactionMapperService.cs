using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;
using AutoMapper;

namespace API.Settlement.Infrastructure.Services
{
	public class TransactionMapperService : ITransactionMapperService
	{
		private readonly IMapper _mapper;
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public TransactionMapperService(IMapper mapper,
									IInfrastructureConstants infrastructureConstants)
		{
			_mapper = mapper;
			_InfrastructureConstants = infrastructureConstants;
		}

		public AvailabilityStockInfoResponseDTO MapToAvailabilityStockInfoResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status)
		{
			var availabilityStockResponseDTO = _mapper.Map<AvailabilityStockInfoResponseDTO>(stockInfoRequestDTO);
			availabilityStockResponseDTO.IsSuccessful = status == Status.Scheduled;
			availabilityStockResponseDTO.Message = GetMessageBasedOnStatus(status);
			availabilityStockResponseDTO.SinglePriceIncludingCommission = GetSinglePriceWithCommission(totalPriceIncludingCommission, availabilityStockResponseDTO.Quantity);

			return availabilityStockResponseDTO;
		}

		public AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
		{
			var availabilityResponseDTO = _mapper.Map<AvailabilityResponseDTO>(finalizeTransactionRequestDTO);
			availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = availabilityStockInfoResponseDTOs;

			return availabilityResponseDTO;
		}

		public FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var finalizeTransactionResponseDTO = _mapper.Map<FinalizeTransactionResponseDTO>(availabilityResponseDTO);
			var stockInfoResponseDTOs = _mapper.Map<IEnumerable<StockInfoResponseDTO>>(availabilityResponseDTO.AvailabilityStockInfoResponseDTOs);
			finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

			return finalizeTransactionResponseDTO;
		}

		public AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfoDTOs(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var successfulAvailabilityStockInfoDTOs = availabilityResponseDTO.AvailabilityStockInfoResponseDTOs.Where(x => x.IsSuccessful).ToList();

			availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = successfulAvailabilityStockInfoDTOs;

			return availabilityResponseDTO;
		}

		public AvailabilityResponseDTO CloneAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			return _mapper.Map<AvailabilityResponseDTO>(availabilityResponseDTO);
		}


		private decimal GetSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity)
		{
			return totalPriceIncludingCommission / quantity;
		}

		private string GetMessageBasedOnStatus(Status status)
		{
			switch (status)
			{
				case Status.Declined: return _InfrastructureConstants.TransactionDeclinedMessage;
				case Status.Success: return _InfrastructureConstants.TransactionSuccessMessage;
				case Status.Scheduled: return _InfrastructureConstants.TransactionScheduledMessage;
				default: return String.Empty;

			}
		}

		public IEnumerable<Transaction> MapToTransactionEntities(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{

			var transactions = _mapper.Map<IEnumerable<Transaction>>(finalizeTransactionResponseDTO.StockInfoResponseDTOs);
			foreach (var transaction in transactions)
			{
				transaction.WalletId = finalizeTransactionResponseDTO.WalletId;
				transaction.UserId = finalizeTransactionResponseDTO.UserId;
				transaction.IsSale = finalizeTransactionResponseDTO.IsSale;
			}

			return transactions;
		}
		public IEnumerable<FinalizeTransactionResponseDTO> MapToFinalizeTransactionResponseDTOs(IEnumerable<Transaction> transactions)
		{
			var groupedTransactions = transactions
				.GroupBy(transaction => new { transaction.WalletId, transaction.UserId, transaction.IsSale });

			var finalizeTransactionResponseDTOs = new List<FinalizeTransactionResponseDTO>();
			foreach (var currentGroup in groupedTransactions)
			{
				var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
				finalizeTransactionResponseDTO.WalletId = currentGroup.Key.WalletId;
				finalizeTransactionResponseDTO.UserId = currentGroup.Key.UserId;
				finalizeTransactionResponseDTO.IsSale = currentGroup.Key.IsSale;

				var stockInfoResponseDTOs = new List<StockInfoResponseDTO>();
				foreach (var currentTransaction in currentGroup)
				{
					var stockInfoResponseDTO = new StockInfoResponseDTO()
					{
						TransactionId = currentTransaction.TransactionId,
						Message = currentTransaction.Message,
						StockId = currentTransaction.StockId,
						StockName = currentTransaction.StockName,
						Quantity = currentTransaction.Quantity,
						SinglePriceIncludingCommission = currentTransaction.SinglePriceIncludingCommission
					};
					stockInfoResponseDTOs.Add(stockInfoResponseDTO);
				}

				finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

				finalizeTransactionResponseDTOs.Add(finalizeTransactionResponseDTO);
			}

			return finalizeTransactionResponseDTOs;
		}

	}
}