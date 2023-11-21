using API.Settlement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.SQLiteServices
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _connectionString;
        public DatabaseInitializer(string connectionString)
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
            string createSuccessfulTransactionTableQuery = CreateSuccessfulTransactionTableQuery();
            string createFailedTransactionTableQuery = CreateFailedTransactionTableQuery();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = createSuccessfulTransactionTableQuery;
                command.ExecuteNonQuery();

                command.CommandText = createFailedTransactionTableQuery;
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
					IsSale INTEGER,
					Message TEXT,
					PRIMARY KEY(TransactionId)
					);";
        }
    }
}
