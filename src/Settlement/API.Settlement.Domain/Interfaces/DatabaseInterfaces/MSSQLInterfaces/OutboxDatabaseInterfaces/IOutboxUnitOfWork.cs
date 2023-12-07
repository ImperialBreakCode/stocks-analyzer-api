namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxUnitOfWork
	{
		IOutboxPendingMessageRepository PendingMessageRepository { get; }
		IOutboxSuccessfullySentMessageRepository SuccessfullySentMessageRepository { get;}
	}
}
