using API.Accounts.Application.DTOs.Request;

namespace API.Accounts.Application.Services.TransactionService
{
    public interface ITransactionService
    {
        bool CompleteTransactions(FinalizeTransactionDTO finalizeTransactionDTO);
    }
}
