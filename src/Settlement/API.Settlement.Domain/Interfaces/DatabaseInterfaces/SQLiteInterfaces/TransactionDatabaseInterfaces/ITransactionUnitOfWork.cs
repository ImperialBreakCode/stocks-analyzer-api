namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
    public interface ITransactionUnitOfWork
    {
        ISuccessfulTransactionRepository SuccessfulTransactions { get; }
        IFailedTransactionRepository FailedTransactions { get; }
    }
}
