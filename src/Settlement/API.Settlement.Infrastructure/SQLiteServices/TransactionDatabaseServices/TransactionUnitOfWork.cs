using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;

namespace API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices
{
	public class TransactionUnitOfWork : ITransactionUnitOfWork
    {
        public ISuccessfulTransactionRepository SuccessfulTransactions { get; }
        public IFailedTransactionRepository FailedTransactions { get; }
        public TransactionUnitOfWork(ISuccessfulTransactionRepository successfulTransactions,
                                          IFailedTransactionRepository failedTransactions)
        {
            SuccessfulTransactions = successfulTransactions;
            FailedTransactions = failedTransactions;
        }

    }
}
