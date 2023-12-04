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

		public void AddPendingMessage(OutboxPendingMessage outboxPendingMessageEntity)
		{
			string commandText = $@"INSERT INTO PendingMessage
								(Id, QueueType, Body, PendingDateTime) VALUES
								(@Id, @QueueType, @Body, @PendingDateTime)";

			using (SqlCommand command = new SqlCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@Id", outboxPendingMessageEntity.Id);
				command.Parameters.AddWithValue("@QueueType", outboxPendingMessageEntity.QueueType);
				command.Parameters.AddWithValue("@Body", outboxPendingMessageEntity.Body);
				command.Parameters.AddWithValue("@PendingDateTime", outboxPendingMessageEntity.PendingDateTime);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}


		public void DeletePendingMessage(string id)
		{
			string commandText = $@"DELETE FROM PendingMessage WHERE Id = @Id";
			using (SqlCommand command = new SqlCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@Id", id);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public IEnumerable<OutboxPendingMessage> GetAll()
		{
			string commandText = $@"SELECT * FROM PendingMessage";
			using (SqlCommand command = new SqlCommand(commandText, _connection))
			{
				_connection.Open();
				var pendingMessages = new List<OutboxPendingMessage>();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var pendingMessage = new OutboxPendingMessage()
						{
							Id = Convert.ToString(reader["Id"]),
							QueueType = Convert.ToString(reader["QueueType"]),
							Body = Convert.ToString(reader["Body"]),
							PendingDateTime = DateTime.Parse(Convert.ToString(reader["PendingDateTime"]))
						};
						pendingMessages.Add(pendingMessage);
					}
				_connection.Close();
				}
				return pendingMessages;
			}
		}
	}
}
