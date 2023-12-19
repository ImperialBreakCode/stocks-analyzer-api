using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
	public interface ISuccessfulTransactionRepository
    {
        void Add(Transaction transaction);
        void Delete(string transactionId);
        bool ContainsTransaction(string transactionId);

    }
}
