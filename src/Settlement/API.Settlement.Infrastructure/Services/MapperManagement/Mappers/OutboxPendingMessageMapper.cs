using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class OutboxPendingMessageMapper : IOutboxPendingMessageMapper
    {
        public OutboxPendingMessage MapToOutboxPendingMessageEntity(Transaction transaction)
        {
            return new OutboxPendingMessage()
            {
                Id = transaction.TransactionId,
                QueueType = "transactionSellStock",
                Body = JsonConvert.SerializeObject(transaction),
                PendingDateTime = DateTime.UtcNow
            };
        }
    }
}
