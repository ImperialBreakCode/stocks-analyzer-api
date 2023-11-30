using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
{
	public class OutboxAcknowledgedMessageRepository : IOutboxAcknowledgedMessageRepository
	{
		private readonly SqlConnection _connection;

		public OutboxAcknowledgedMessageRepository(SqlConnection connection)
		{
			_connection = connection;
		}

		public void AddAcknowledgedMessage(OutboxAcknowledgedMessageEntity outboxAcknowledgedMessageEntity)
		{
			throw new NotImplementedException();
		}
	}
}
