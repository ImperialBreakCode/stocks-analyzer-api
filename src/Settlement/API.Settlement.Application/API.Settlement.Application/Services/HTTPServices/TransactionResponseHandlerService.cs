using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.HTTPInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.HTTPServices
{
	public class TransactionResponseHandlerService : ITransactionResponseHandlerService
	{
		private readonly ITransactionUnitOfWork _transactionUnitOfWork;
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public TransactionResponseHandlerService(ITransactionUnitOfWork transactionUnitOfWork,
												 IInfrastructureConstants infrastructureConstants)
		{
			_transactionUnitOfWork = transactionUnitOfWork;
			_InfrastructureConstants = infrastructureConstants;
		}

		public void HandleTransactionResponse(HttpResponseMessage response, IEnumerable<Transaction> transactions)
		{
			if (response.IsSuccessStatusCode)
			{
				foreach (var transaction in transactions)
				{
					if (!_transactionUnitOfWork.SuccessfulTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionSuccessMessage;
						_transactionUnitOfWork.SuccessfulTransactions.Add(transaction);
					}

					if (_transactionUnitOfWork.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						_transactionUnitOfWork.FailedTransactions.Delete(transaction.TransactionId);
					}

				}
			}
			else
			{
				foreach (var transaction in transactions)
				{
					if (!_transactionUnitOfWork.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionConnectionIssueMessage;
						_transactionUnitOfWork.FailedTransactions.Add(transaction);
					}

				}
			}
		}

	}
}