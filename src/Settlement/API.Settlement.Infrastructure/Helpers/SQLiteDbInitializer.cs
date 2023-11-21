using System.Data.SQLite;

namespace API.Settlement.Extensions
{
	public static class SQLiteDbInitializer
	{
		private static string _connectionString = @"Data Source=D:\Университет\Втори курс\Трети семестър\API\stocks-analyzer-api\src\Settlement\API.Settlement.Domain\SQLite\TransactionsDB.db";
		public static void Initialize()
		{
			using (var connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				CreateTables(connection);
			}
		}

		private static void CreateTables(SQLiteConnection connection)
		{
			string createSuccessfulTransactionTableQuery = CreateSuccessfulTransactionTableQuery();
			string createFailedTransactionTableQuery = CreateFailedTransactionTableQuery();
			using (var command = new SQLiteCommand(connection))
			{
				command.CommandText = createSuccessfulTransactionTableQuery;
				command.ExecuteNonQuery();

				command.CommandText += createFailedTransactionTableQuery;
				command.ExecuteNonQuery();
			}

		}

		private static string CreateSuccessfulTransactionTableQuery()
		{
			return @"CREATE TABLE IF NOT EXISTS SuccessfulTransaction (
															TransactionId INTEGER NOT NULL UNIQUE,
															TotalAmount REAL,
															Quantity INTEGER,
															DateTime TEXT,
															StockId INTEGER,
															WalletId INTEGER,
															IsCompleted INTEGER,
															PRIMARY KEY(TransactionId AUTOINCREMENT)
														);";
		}
		private static string CreateFailedTransactionTableQuery()
		{
			return @"CREATE TABLE IF NOT EXISTS FailedTransaction (
															TransactionId INTEGER NOT NULL UNIQUE,
															TotalAmount REAL,
															Quantity INTEGER,
															DateTime TEXT,
															StockId INTEGER,
															WalletId INTEGER,
															IsCompleted INTEGER,
															PRIMARY KEY(TransactionId AUTOINCREMENT)
														);";
		}
	}
}
