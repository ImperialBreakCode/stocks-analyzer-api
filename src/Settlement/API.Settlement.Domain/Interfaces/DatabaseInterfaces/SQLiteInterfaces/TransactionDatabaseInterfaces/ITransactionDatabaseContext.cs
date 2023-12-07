namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
    public interface ITransactionDatabaseContext
    {
        ISuccessfulTransactionRepository SuccessfulTransactions { get; }
        IFailedTransactionRepository FailedTransactions { get; }
    }
}
