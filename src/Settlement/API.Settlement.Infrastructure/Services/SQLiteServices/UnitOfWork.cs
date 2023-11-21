using API.Settlement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.SQLiteServices
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISuccessfulTransactionRepository SuccessfulTransactions { get; }

        public IFailedTransactionRepository FailedTransactions { get; }
        public UnitOfWork(ISuccessfulTransactionRepository successfulTransactions, IFailedTransactionRepository failedTransactions)
        {
            SuccessfulTransactions = successfulTransactions;
            FailedTransactions = failedTransactions;
        }
    }
}
