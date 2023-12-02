using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxPendingMessageRepository
	{
		void AddPendingMessage(OutboxPendingMessageEntity message);
		void DeletePendingMessage(string id);
		IEnumerable<OutboxPendingMessageEntity> GetAll();
	}
}
