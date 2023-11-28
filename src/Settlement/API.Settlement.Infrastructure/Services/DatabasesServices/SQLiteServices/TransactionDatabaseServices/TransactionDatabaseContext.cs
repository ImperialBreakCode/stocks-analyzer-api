using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.SQLiteServices
{
    public class TransactionDatabaseContext : ITransactionDatabaseContext
    {
        public ISuccessfulTransactionRepository SuccessfulTransactions { get; }

        public IFailedTransactionRepository FailedTransactions { get; }
        public TransactionDatabaseContext(ISuccessfulTransactionRepository successfulTransactions, 
                                        IFailedTransactionRepository failedTransactions)
        {
            SuccessfulTransactions = successfulTransactions;
            FailedTransactions = failedTransactions;
        }
    }
}
