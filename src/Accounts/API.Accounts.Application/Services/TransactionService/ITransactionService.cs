using API.Accounts.Application.DTOs.RabbitMQConsumerDTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.TransactionService
{
    public interface ITransactionService
    {
        bool CompleteTransactions(FinalizeTransactionDTO finalizeTransactionDTO);
        ICollection<GetTransactionResponseDTO> GetTransactionsByWalletId(string walletId);
        ICollection<GetTransactionResponseDTO> GetTransactionsByUsername(string username);
        void CreateSaleTransaction(TransactionConsumeDTO transactionConsumeDTO);
    }
}
