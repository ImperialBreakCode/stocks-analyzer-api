using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices
{
	public class SQLiteOutboxDatabaseInitializer : ISQLiteOutboxDatabaseInitializer
	{
		private readonly string _connectionString;

		public SQLiteOutboxDatabaseInitializer(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void Initialize()
		{
			using (var connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();
				CreateTables(connection);
				connection.Close();
			}
		}
		private void CreateTables(SQLiteConnection connection)
		{
			string createOutboxPendingMessageTableQuery = CreateOutboxPendingMessageTableQuery();
			string createAcknowledgedMessageTableQuery = CreateAcknowledgedMessageTableQuery();

			using (var command = new SQLiteCommand(connection))
			{
				command.CommandText = createOutboxPendingMessageTableQuery;
				command.ExecuteNonQuery();

				command.CommandText = createAcknowledgedMessageTableQuery;
				command.ExecuteNonQuery();
			}
		}

		private string CreateOutboxPendingMessageTableQuery()
		{
			return @"CREATE TABLE IF NOT EXISTS PendingMessage (
                Id TEXT NOT NULL UNIQUE,
                MessageType TEXT NOT NULL,
                Body TEXT NOT NULL,
                PendingDateTime DATETIME NOT NULL,
				PRIMARY KEY(Id)
            );";
		}
		private string CreateAcknowledgedMessageTableQuery()
		{
			return @"CREATE TABLE IF NOT EXISTS AcknowledgedMessage (
                Id TEXT NOT NULL UNIQUE,
                AcknowledgedInfo TEXT,
                AcknowledgedDateTime DATETIME NOT NULL,
				PRIMARY KEY(Id)
            );";
		}

	}
}
