using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
{
	public class OutboxPendingMessageRepository : IOutboxPendingMessageRepository
	{
		private readonly SqlConnection _connection;

		public OutboxPendingMessageRepository(SqlConnection connection)
		{
			_connection = connection;
		}

		public void AddPendingMessage(OutboxPendingMessageEntity outboxPendingMessageEntity)
		{
			string commandText = $@"INSERT INTO PendingMessage
								(Id, MessageType, Body, PendingDateTime) VALUES
								(@Id, @MessageType, @Body, @PendingDateTime)";

			using (SqlCommand command = new SqlCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@Id", outboxPendingMessageEntity.Id);
				command.Parameters.AddWithValue("@MessageType", outboxPendingMessageEntity.MessageType);
				command.Parameters.AddWithValue("@Body", JsonConvert.SerializeObject(outboxPendingMessageEntity.Body));
				command.Parameters.AddWithValue("@PendingDateTime", outboxPendingMessageEntity.PendingDateTime);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}
		

		public void DeletePendingMessage(string id)
		{
			string commandText = $@"DELETE FROM PendingMessage WHERE TransactionId = @TransactionId";
			using (SqlCommand command = new SqlCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@TransactionId", id);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}

	}
}
