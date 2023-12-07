namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxDatabaseContext
	{
		IOutboxPendingMessageRepository PendingMessageRepository { get; }
		IOutboxSuccessfullySentMessageRepository SuccessfullySentMessageRepository { get;}
	}
}
