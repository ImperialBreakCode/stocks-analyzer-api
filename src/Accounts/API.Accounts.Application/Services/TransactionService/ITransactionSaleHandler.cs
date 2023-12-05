using RabbitMQ.Client.Events;

namespace API.Accounts.Application.Services.TransactionService
{
    public interface ITransactionSaleHandler
    {
        void HandleSale(object? model, BasicDeliverEventArgs args);
    }
}
