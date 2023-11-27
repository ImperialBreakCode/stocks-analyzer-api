﻿using API.Settlement.Domain.DTOs.Response;
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
		private readonly ITransactionDatabaseContext _unitOfWork;
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public TransactionResponseHandlerService(ITransactionDatabaseContext unitOfWork, IInfrastructureConstants infrastructureConstants)
		{
			_unitOfWork = unitOfWork;
			_InfrastructureConstants = infrastructureConstants;
		}
		public void HandleTransactionResponse(HttpResponseMessage response, IEnumerable<Transaction> transactions)
		{
			//if(response.IsSuccessStatusCode)
			if (response.StatusCode == HttpStatusCode.OK)
			{
				foreach (var transaction in transactions)
				{
					if (!_unitOfWork.SuccessfulTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionSuccessMessage;
						_unitOfWork.SuccessfulTransactions.Add(transaction);
					}

					if (_unitOfWork.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						_unitOfWork.FailedTransactions.Delete(transaction.TransactionId);
					}

				}
			}
			else
			{
				foreach (var transaction in transactions)
				{
					if (!_unitOfWork.FailedTransactions.ContainsTransaction(transaction.TransactionId))
					{
						transaction.Message = _InfrastructureConstants.TransactionConnectionIssueMessage;
						_unitOfWork.FailedTransactions.Add(transaction);
					}

				}
			}



		}
	}
}