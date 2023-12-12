using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Interfaces.CommissionInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Mappings.Mappers
{
	public class TransactionMapper : ITransactionMapper
    {
        private readonly IMapper _mapper;
        private readonly IConstantsHelperWrapper _infrastructureConstants;
        private readonly IUserCommissionCalculatorHelper _userCommissionCalculatorHelper;
        public TransactionMapper(IMapper mapper,
                                 IConstantsHelperWrapper infrastructureConstants,
                                 IUserCommissionCalculatorHelper userCommissionCalculatorHelper)
        {
            _mapper = mapper;
            _infrastructureConstants = infrastructureConstants;
            _userCommissionCalculatorHelper = userCommissionCalculatorHelper;
        }
        public IEnumerable<Transaction> MapToTransactionEntities(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
        {

            var transactions = _mapper.Map<IEnumerable<Transaction>>(finalizeTransactionResponseDTO.StockInfoResponseDTOs);
            foreach (var transaction in transactions)
            {
                transaction.WalletId = finalizeTransactionResponseDTO.WalletId;
                transaction.UserId = finalizeTransactionResponseDTO.UserId;
                transaction.UserEmail = finalizeTransactionResponseDTO.UserEmail;
                transaction.IsSale = finalizeTransactionResponseDTO.IsSale;
                transaction.UserRank = finalizeTransactionResponseDTO.UserRank;
            }

            return transactions;
        }
        public Transaction MapToSelllTransactionEntity(Wallet wallet, Stock stock, decimal actualTotalStockPrice)
        {
            var transaction = _mapper.Map<Transaction>(wallet);
            transaction = _mapper.Map(stock, transaction);
            transaction.TransactionId = Guid.NewGuid().ToString();
            transaction.IsSale = true;
            transaction.Message = _infrastructureConstants.MessageConstants.TransactionScheduledMessage;
            transaction.TotalPriceIncludingCommission = _userCommissionCalculatorHelper.CalculatePriceAfterAddingSaleCommission(actualTotalStockPrice, wallet.UserRank);
            return transaction;
        }
    }
}
