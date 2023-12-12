using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using AutoMapper;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;

namespace API.Settlement.Application.Mappings.Mappers
{
	public class FinalizeTransactionResponseDTOMapper : IFinalizeTransactionResponseDTOMapper
    {
        private readonly IMapper _mapper;
        private readonly IConstantsHelperWrapper _infrastructureConstants;
		public FinalizeTransactionResponseDTOMapper(IMapper mapper, IConstantsHelperWrapper infrastructureConstants)
		{
			_mapper = mapper;
			_infrastructureConstants = infrastructureConstants;
		}

		public FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
        {
            var finalizeTransactionResponseDTO = _mapper.Map<FinalizeTransactionResponseDTO>(availabilityResponseDTO);
            var stockInfoResponseDTOs = _mapper.Map<IEnumerable<StockInfoResponseDTO>>(availabilityResponseDTO.AvailabilityStockInfoResponseDTOs);
            finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

            return finalizeTransactionResponseDTO;
        }

        public FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(Transaction transaction)
        {
            var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
            finalizeTransactionResponseDTO.WalletId = transaction.WalletId;
            finalizeTransactionResponseDTO.UserId = transaction.UserId;
            finalizeTransactionResponseDTO.UserEmail = transaction.UserEmail;
            finalizeTransactionResponseDTO.IsSale = transaction.IsSale;
            finalizeTransactionResponseDTO.StockInfoResponseDTOs = new List<StockInfoResponseDTO>();
            var stockInfoResponseDTOs = new List<StockInfoResponseDTO>();
            var stockInfoResponseDTO = new StockInfoResponseDTO()
            {
                TransactionId = transaction.TransactionId,
                Message = transaction.Message,
                StockId = transaction.StockId,
                StockName = transaction.StockName,
                Quantity = transaction.Quantity,
                SinglePriceIncludingCommission = transaction.SinglePriceIncludingCommission
            };
            stockInfoResponseDTOs.Add(stockInfoResponseDTO);
            finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;
            return finalizeTransactionResponseDTO;
        }

        public IEnumerable<FinalizeTransactionResponseDTO> MapToFinalizeTransactionResponseDTOs(IEnumerable<Transaction> transactions)
        {
            var groupedTransactions = transactions
                .GroupBy(transaction => new { transaction.WalletId, transaction.UserId, transaction.UserEmail, transaction.IsSale, transaction.UserRank });

            var finalizeTransactionResponseDTOs = new List<FinalizeTransactionResponseDTO>();
            foreach (var currentGroup in groupedTransactions)
            {
                var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
                finalizeTransactionResponseDTO.WalletId = currentGroup.Key.WalletId;
                finalizeTransactionResponseDTO.UserId = currentGroup.Key.UserId;
                finalizeTransactionResponseDTO.UserEmail = currentGroup.Key.UserEmail;
                finalizeTransactionResponseDTO.IsSale = currentGroup.Key.IsSale;
                finalizeTransactionResponseDTO.UserRank = currentGroup.Key.UserRank;

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

		public FinalizeTransactionResponseDTO UpdateStockInfoMessageToSuccessful(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			foreach (var stockInfoResponseDTO in finalizeTransactionResponseDTO.StockInfoResponseDTOs)
			{
				stockInfoResponseDTO.Message = _infrastructureConstants.MessageConstants.TransactionSuccessMessage;
			}
            return finalizeTransactionResponseDTO;
		}
	}
}
