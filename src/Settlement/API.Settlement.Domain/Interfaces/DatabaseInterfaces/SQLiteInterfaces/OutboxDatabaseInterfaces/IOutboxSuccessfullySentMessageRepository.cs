using API.Settlement.Domain.Entities.OutboxEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxSuccessfullySentMessageRepository
	{
		void AddSuccessfullySentMessage(OutboxSuccessfullySentMessage outboxAcknowledgedMessageEntity);
	}
}
