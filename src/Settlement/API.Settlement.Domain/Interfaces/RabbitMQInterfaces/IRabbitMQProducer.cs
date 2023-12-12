using API.Settlement.Domain.Entities.OutboxEntities;

namespace API.Settlement.Domain.Interfaces.RabbitMQInterfaces
{
	public interface IRabbitMQProducer
	{
		void SendMessage(OutboxPendingMessage outboxPendingMessageEntity);
	}
}
