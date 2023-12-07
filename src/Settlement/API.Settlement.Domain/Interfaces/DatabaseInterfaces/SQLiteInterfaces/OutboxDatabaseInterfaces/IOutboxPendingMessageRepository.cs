using API.Settlement.Domain.Entities.OutboxEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxPendingMessageRepository
	{
		void AddPendingMessage(OutboxPendingMessage message);
		void DeletePendingMessage(string id);
		IEnumerable<OutboxPendingMessage> GetAll();
	}
}
