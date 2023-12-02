using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services
{
    public class TransactionResponseHandlerService : ITransactionResponseHandlerService
	{
		private readonly ITransactionDatabaseContext _transactionDatabaseContext;
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public TransactionResponseHandlerService(ITransactionDatabaseContext transactionDatabaseContext, IInfrastructureConstants infrastructureConstants)
		{
			_transactionDatabaseContext = transactionDatabaseContext;
			_InfrastructureConstants = infrastructureConstants;
		}
		public void HandleTransactionResponse(HttpResponseMessage response, IEnumerable<Transaction> transactions)
		{
			if(response.IsSuccessStatusCode)
			{
				foreach (var transaction in transactions)
				{
					if (!_transactionDatabaseContext.SuccessfulTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionSuccessMessage;
						_transactionDatabaseContext.SuccessfulTransactions.Add(transaction);
					}

					if (_transactionDatabaseContext.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						_transactionDatabaseContext.FailedTransactions.Delete(transaction.TransactionId);
					}

				}
			}
			else
			{
				foreach (var transaction in transactions)
				{
					if (!_transactionDatabaseContext.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionConnectionIssueMessage;
						_transactionDatabaseContext.FailedTransactions.Add(transaction);
					}

				}
			}



		}
	}
}
