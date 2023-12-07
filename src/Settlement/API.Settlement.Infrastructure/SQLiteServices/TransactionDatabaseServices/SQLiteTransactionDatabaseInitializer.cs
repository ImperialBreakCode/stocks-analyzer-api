using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using System.Data.SQLite;

namespace API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices
{
	public class SQLiteTransactionDatabaseInitializer : ISQLiteTransactionDatabaseInitializer
    {
        private readonly string _connectionString;
        public SQLiteTransactionDatabaseInitializer(string connectionString)
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
            string successfulTransactionTableQuery = CreateSuccessfulTransactionTableQuery();
            string failedTransactionTableQuery = CreateFailedTransactionTableQuery();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = successfulTransactionTableQuery;
                command.ExecuteNonQuery();

                command.CommandText = failedTransactionTableQuery;
                command.ExecuteNonQuery();
            }
        }

        private string CreateSuccessfulTransactionTableQuery()
        {
            return @"CREATE TABLE IF NOT EXISTS SuccessfulTransaction (
					TransactionId TEXT NOT NULL UNIQUE,
					TotalPriceIncludingCommission REAL,
					Quantity INTEGER,
					DateTime TEXT,
					StockName TEXT,
					StockId TEXT,
					UserId TEXT,
					WalletId TEXT,
                    UserEmail TEXT,
					IsSale INTEGER,
					Message TEXT,
					PRIMARY KEY(TransactionId)
					);";
        }

        private string CreateFailedTransactionTableQuery()
        {
            return @"CREATE TABLE IF NOT EXISTS FailedTransaction (
					TransactionId TEXT NOT NULL UNIQUE,
					TotalPriceIncludingCommission REAL,
					Quantity INTEGER,
					DateTime TEXT,
					StockName TEXT,
					StockId TEXT,
					UserId TEXT,
					WalletId TEXT,
                    UserEmail TEXT,
					IsSale INTEGER,
					Message TEXT,
					PRIMARY KEY(TransactionId)
					);";
        }
    }
}
