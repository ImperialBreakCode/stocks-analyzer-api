using API.Settlement.Domain.Entities.OutboxEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface IOutboxSuccessfullySentMessageMapper
    {
        OutboxSuccessfullySentMessage MapToOutboxSuccessfullySentMessageEntity(OutboxPendingMessage outboxPendingMessageEntity);
    }
}
