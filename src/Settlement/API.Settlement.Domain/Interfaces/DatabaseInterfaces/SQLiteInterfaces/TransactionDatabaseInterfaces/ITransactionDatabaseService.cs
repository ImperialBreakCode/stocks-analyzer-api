using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
	public interface ITransactionDatabaseService
	{
		void DeleteTransactionsWithWalletId(string walletId);
	}
}
