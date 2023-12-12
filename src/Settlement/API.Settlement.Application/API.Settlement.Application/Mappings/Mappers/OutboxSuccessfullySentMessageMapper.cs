using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Mappings.Mappers
{
    public class OutboxSuccessfullySentMessageMapper : IOutboxSuccessfullySentMessageMapper
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeHelper _dateTimeHelper;
        public OutboxSuccessfullySentMessageMapper(IMapper mapper,
                                                   IDateTimeHelper dateTimeHelper)
        {
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
        }

        public OutboxSuccessfullySentMessage MapToOutboxSuccessfullySentMessageEntity(OutboxPendingMessage outboxPendingMessageEntity)
        {
            var outboxSuccessfullySentMessageEntity = _mapper.Map<OutboxSuccessfullySentMessage>(outboxPendingMessageEntity);
            outboxSuccessfullySentMessageEntity.SentDateTime = _dateTimeHelper.UtcNow;
            return outboxSuccessfullySentMessageEntity;
        }
    }
}
