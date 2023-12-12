using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
	public interface IFailedTransactionRepository
    {
        void Add(Transaction transaction);
        Transaction Delete(string transactionId);
        IEnumerable<Transaction> GetAll();
        bool ContainsTransaction(string transactionId);
		bool ContainsTransactionsWithWalletId(string walletId);
		void DeleteTransactionsWithWalletId(string walletId);
	}
}
