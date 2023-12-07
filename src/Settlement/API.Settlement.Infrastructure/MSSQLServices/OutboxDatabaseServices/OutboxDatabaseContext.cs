using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;

namespace API.Settlement.Infrastructure.MSSQLServices.OutboxDatabaseServices
{
    public class OutboxDatabaseContext : IOutboxDatabaseContext
    {
        public IOutboxPendingMessageRepository PendingMessageRepository { get; }
        public IOutboxSuccessfullySentMessageRepository SuccessfullySentMessageRepository { get; }

        public OutboxDatabaseContext(IOutboxPendingMessageRepository pendingMessageRepository,
                                     IOutboxSuccessfullySentMessageRepository acknowledgedMessageRepository)
        {
            PendingMessageRepository = pendingMessageRepository;
            SuccessfullySentMessageRepository = acknowledgedMessageRepository;
        }

    }
}
