using API.Accounts.Application.DTOs.RabbitMQConsumerDTOs;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;

namespace API.Accounts.Application.Services.TransactionService
{
    internal class TransactionSaleHandler : ITransactionSaleHandler
    {
        private readonly ITransactionService _transactionService;

        public TransactionSaleHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void HandleSale(object? model, BasicDeliverEventArgs args)
        {
            byte[] body = args.Body.ToArray();
            string jsonData = Encoding.UTF8.GetString(body);
            var transactionConsumeDTO = JsonConvert.DeserializeObject<TransactionConsumeDTO>(jsonData);

            _transactionService.CreateSaleTransaction(transactionConsumeDTO!);
        }
    }
}
