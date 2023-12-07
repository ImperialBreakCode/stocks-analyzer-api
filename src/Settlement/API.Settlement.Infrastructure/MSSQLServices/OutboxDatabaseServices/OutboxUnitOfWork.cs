using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;

namespace API.Settlement.Infrastructure.MSSQLServices.OutboxDatabaseServices
{
    public class OutboxUnitOfWork : IOutboxUnitOfWork
    {
        public IOutboxPendingMessageRepository PendingMessageRepository { get; }
        public IOutboxSuccessfullySentMessageRepository SuccessfullySentMessageRepository { get; }

        public OutboxUnitOfWork(IOutboxPendingMessageRepository pendingMessageRepository,
                                     IOutboxSuccessfullySentMessageRepository acknowledgedMessageRepository)
        {
            PendingMessageRepository = pendingMessageRepository;
            SuccessfullySentMessageRepository = acknowledgedMessageRepository;
        }

    }
}
