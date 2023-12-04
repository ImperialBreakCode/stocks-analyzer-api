using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.RabbitMQServices
{
	public class RabbitMQService : IRabbitMQService
	{
		private readonly IOutboxDatabaseContext _outboxDatabaseContext;
		private readonly IRabbitMQProducer _rabbitMQProducer;
		private readonly IMapperManagementWrapper _transactionMapperService;

		public RabbitMQService(IOutboxDatabaseContext outboxDatabaseContext,
							IRabbitMQProducer rabbitMQSellTransactionProducer,
							IMapperManagementWrapper transactionMapperService)
		{
			_outboxDatabaseContext = outboxDatabaseContext;
			_rabbitMQProducer = rabbitMQSellTransactionProducer;
			_transactionMapperService = transactionMapperService;
		}

		public void PerformMessageSending()
		{
			var outboxPendingMessageEntities =  _outboxDatabaseContext.PendingMessageRepository.GetAll();
			foreach (var outboxPendingMessageEntity in outboxPendingMessageEntities)
			{
				_rabbitMQProducer.SendMessage(outboxPendingMessageEntity);
				_outboxDatabaseContext.PendingMessageRepository.DeletePendingMessage(outboxPendingMessageEntity.Id);

				var outboxSuccessfullySentMessageEntity = _transactionMapperService.OutboxSuccessfullySentMessageMapper.MapToOutboxSuccessfullySentMessageEntity(outboxPendingMessageEntity);
				_outboxDatabaseContext.SuccessfullySentMessageRepository.AddSuccessfullySentMessage(outboxSuccessfullySentMessageEntity);
			}
		}

	}
}
