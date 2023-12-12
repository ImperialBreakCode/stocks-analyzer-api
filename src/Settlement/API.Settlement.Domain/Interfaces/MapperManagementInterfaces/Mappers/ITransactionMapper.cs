using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
	public interface ITransactionMapper
    {
        IEnumerable<Transaction> MapToTransactionEntities(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
        Transaction MapToSelllTransactionEntity(Wallet wallet, Stock stock, decimal actualTotalStockPrice);
    }
}
