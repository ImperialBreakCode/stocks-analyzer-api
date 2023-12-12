using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.RabbitMQServices
{
	public class RabbitMQService : IRabbitMQService
	{
		private readonly IOutboxUnitOfWork _outboxUnitOfWork;
		private readonly IRabbitMQProducer _rabbitMQProducer;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;

		public RabbitMQService(IOutboxUnitOfWork outboxUnitOfWork,
							   IRabbitMQProducer rabbitMQSellTransactionProducer,
							   IMapperManagementWrapper mapperManagementWrapper)
		{
			_outboxUnitOfWork = outboxUnitOfWork;
			_rabbitMQProducer = rabbitMQSellTransactionProducer;
			_mapperManagementWrapper = mapperManagementWrapper;
		}

		public void PerformMessageSending()
		{
			var outboxPendingMessageEntities = _outboxUnitOfWork.PendingMessageRepository.GetAll();
			foreach (var outboxPendingMessageEntity in outboxPendingMessageEntities)
			{
				_rabbitMQProducer.SendMessage(outboxPendingMessageEntity);
				_outboxUnitOfWork.PendingMessageRepository.DeletePendingMessage(outboxPendingMessageEntity.Id);

				var outboxSuccessfullySentMessageEntity = _mapperManagementWrapper.OutboxSuccessfullySentMessageMapper.MapToOutboxSuccessfullySentMessageEntity(outboxPendingMessageEntity);
				_outboxUnitOfWork.SuccessfullySentMessageRepository.AddSuccessfullySentMessage(outboxSuccessfullySentMessageEntity);
			}
		}

	}
}
