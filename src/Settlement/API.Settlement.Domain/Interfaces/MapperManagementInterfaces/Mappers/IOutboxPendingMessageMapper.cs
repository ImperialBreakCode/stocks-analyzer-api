using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
	public interface IOutboxPendingMessageMapper
    {
        OutboxPendingMessage MapToOutboxPendingMessageEntity(Transaction transaction);
    }
}
