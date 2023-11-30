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
				CreateTables(connection);
				connection.Close();
			}
		}

		private void CreateTables(SqlConnection connection)
		{
			string createOutboxPendingMessageTableQuery = CreateOutboxPendingMessageTableQuery();
			string createAcknowledgedMessageTableQuery = CreateAcknowledgedMessageTableQuery();

			using (var command = new SqlCommand())
			{
				command.Connection = connection;
				command.CommandType = CommandType.Text;
				command.CommandText = createOutboxPendingMessageTableQuery;
				command.ExecuteNonQuery();

				command.CommandText = createAcknowledgedMessageTableQuery;
				command.ExecuteNonQuery();
			}
		}

		private string CreateOutboxPendingMessageTableQuery()
		{
			return @"CREATE TABLE PendingMessage (
                Id NVARCHAR(255) NOT NULL PRIMARY KEY,
                MessageType NVARCHAR(255) NOT NULL,
                Body NVARCHAR(MAX) NOT NULL,
                PendingDateTime DATETIME NOT NULL
            );";
		}

		private string CreateAcknowledgedMessageTableQuery()
		{
			return @"CREATE TABLE AcknowledgedMessage (
                Id NVARCHAR(255) NOT NULL PRIMARY KEY,
                AcknowledgedInfo NVARCHAR(MAX),
                AcknowledgedDateTime DATETIME NOT NULL
            );";
		}
	}

}
