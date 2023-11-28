using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces
{
    public interface IFailedTransactionRepository
    {
        void Add(Transaction transaction);
        void Delete(string transactionId);
        IEnumerable<Transaction> GetAll();
        bool ContainsTransaction(string transactionId);
    }
}
