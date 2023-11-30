using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
{
	public class OutboxPendingMessageRepository : IOutboxPendingMessageRepository
	{
		private readonly SQLiteConnection _connection;
		private readonly IDateTimeService _dateTimeService;

		public OutboxPendingMessageRepository(SQLiteConnection connection, 
											IDateTimeService dateTimeService)
		{
			_connection = connection;
			_dateTimeService = dateTimeService;
		}

		public void AddPendingMessage(Transaction message)
		{
			string commandText = $@"INSERT INTO PendingMessage
								(Id, MessageType, Body, PendingDateTime) VALUES
								(@Id, @MessageType, @body, @PendingDateTime)";

			using(SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@Id", new Guid().ToString());
				command.Parameters.AddWithValue("@MessageType", "transactionSellStock");
				command.Parameters.AddWithValue("@Body", JsonConvert.SerializeObject(message));
				command.Parameters.AddWithValue("@PendingDateTime", _dateTimeService.UtcNow);
				command.ExecuteNonQuery();
			}
			_connection.Close();
		}
	}
}
