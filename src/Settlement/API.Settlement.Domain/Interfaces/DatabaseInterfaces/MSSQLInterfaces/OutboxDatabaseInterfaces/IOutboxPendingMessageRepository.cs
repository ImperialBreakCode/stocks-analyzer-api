﻿using API.Settlement.Domain.Entities.OutboxEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces
{
	public interface IOutboxPendingMessageRepository
	{
		void AddPendingMessage(OutboxPendingMessage message);
		void DeletePendingMessage(string id);
		IEnumerable<OutboxPendingMessage> GetAll();
	}
}
