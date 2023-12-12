namespace API.Settlement.Domain.Interfaces.TransactionInterfaces.TransactionCompletionInterfaces
{
    public interface IFailedTransactionCompletionService
    {
        Task ProcessFailedTransactions();
    }
}
