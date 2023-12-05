using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
{
	public class MSSQLOutboxDatabaseInitializer : IMSSQLOutboxDatabaseInitializer
	{
		private readonly string _connectionString;

		public MSSQLOutboxDatabaseInitializer(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void Initialize()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				if (!IsSendingMessageTableAlreadyInitialized(connection)) { CreateSendingMessageTable(connection); }
				if (!IsSuccessfullySentMessageTableAlreadyInitialized(connection)) { CreateSuccessfullySentMessageTable(connection); }

				connection.Close();
			}
		}

		private bool IsSendingMessageTableAlreadyInitialized(SqlConnection connection)
		{
			int sendingMessageTableColumnsCount = 0;
			using (var command = new SqlCommand())
			{
				command.Connection = connection;
				command.CommandType = CommandType.Text;
				command.CommandText = CreateCheckSendingMessageTableQuery();
				sendingMessageTableColumnsCount = (int)command.ExecuteScalar();
			}
			return sendingMessageTableColumnsCount > 0;
		}

		private void CreateSendingMessageTable(SqlConnection connection)
		{
			using (var command = new SqlCommand())
			{
				command.Connection = connection;
				command.CommandType = CommandType.Text;

				command.CommandText = CreatePendingMessageTableQuery();
				command.ExecuteNonQuery();
			}
		}

		private bool IsSuccessfullySentMessageTableAlreadyInitialized(SqlConnection connection)
		{
			int successfullySentMessageTableColumnsCount = 0;
			using (var command = new SqlCommand())
			{
				command.Connection = connection;
				command.CommandType = CommandType.Text;
				command.CommandText = CreateCheckSuccessfullySentMessageTableQuery();
				successfullySentMessageTableColumnsCount = (int)command.ExecuteScalar();
			}
			return successfullySentMessageTableColumnsCount > 0;
		}

		private void CreateSuccessfullySentMessageTable(SqlConnection connection)
		{
			using (var command = new SqlCommand())
			{
				command.Connection = connection;
				command.CommandType = CommandType.Text;

				command.CommandText = CreateSuccessfullySentMessageTableQuery();
				command.ExecuteNonQuery();
			}
		}

		private string CreateCheckSendingMessageTableQuery()
		{
			return $@"SELECT COUNT(*)
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_NAME = 'PendingMessage';";
		}

		private string CreateCheckSuccessfullySentMessageTableQuery()
		{
			return $@"SELECT COUNT(*)
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_NAME = 'SuccessfullySentMessage';";
		}

		private string CreatePendingMessageTableQuery()
		{
			return @"CREATE TABLE PendingMessage (
                Id NVARCHAR(255) NOT NULL PRIMARY KEY,
                QueueType NVARCHAR(255) NOT NULL,
                Body NVARCHAR(MAX) NOT NULL,
                PendingDateTime DATETIME NOT NULL
            );";
		}

		private string CreateSuccessfullySentMessageTableQuery()
		{
			return @"CREATE TABLE SuccessfullySentMessage (
                Id NVARCHAR(255) NOT NULL PRIMARY KEY,
				QueueType NVARCHAR(255) NOT NULL,
                SentInfo NVARCHAR(MAX),
                SentDateTime DATETIME NOT NULL
            );";
		}

	}

}
