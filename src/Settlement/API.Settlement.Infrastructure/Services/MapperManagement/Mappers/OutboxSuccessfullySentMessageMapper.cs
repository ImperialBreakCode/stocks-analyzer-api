using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class OutboxSuccessfullySentMessageMapper : IOutboxSuccessfullySentMessageMapper
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        public OutboxSuccessfullySentMessageMapper(IMapper mapper, 
                                                   IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public OutboxSuccessfullySentMessage MapToOutboxSuccessfullySentMessageEntity(OutboxPendingMessage outboxPendingMessageEntity)
        {
            var outboxSuccessfullySentMessageEntity = _mapper.Map<OutboxSuccessfullySentMessage>(outboxPendingMessageEntity);
            outboxSuccessfullySentMessageEntity.SentDateTime = _dateTimeService.UtcNow;
            return outboxSuccessfullySentMessageEntity;
        }
    }
}
