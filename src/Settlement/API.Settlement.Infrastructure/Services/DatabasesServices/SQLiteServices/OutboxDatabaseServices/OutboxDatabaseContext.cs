using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
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
