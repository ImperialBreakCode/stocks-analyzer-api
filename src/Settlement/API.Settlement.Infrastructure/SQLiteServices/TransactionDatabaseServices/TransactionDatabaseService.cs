using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices
{
	public class TransactionDatabaseService : ITransactionDatabaseService
	{
		private readonly ITransactionUnitOfWork _transactionUnitOfWork;

		public TransactionDatabaseService(ITransactionUnitOfWork transactionUnitOfWork)
		{
			_transactionUnitOfWork = transactionUnitOfWork;
		}

		public void DeleteTransactionsWithWalletId(string walletId)
		{
			if (_transactionUnitOfWork.FailedTransactions.ContainsTransactionsWithWalletId(walletId))
			{
				_transactionUnitOfWork.FailedTransactions.DeleteTransactionsWithWalletId(walletId);
			}
		}
	}
}
