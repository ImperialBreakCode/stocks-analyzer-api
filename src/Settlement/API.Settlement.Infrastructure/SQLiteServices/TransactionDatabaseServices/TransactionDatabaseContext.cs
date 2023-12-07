using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;

namespace API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices
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
