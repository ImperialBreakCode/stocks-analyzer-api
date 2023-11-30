﻿using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.SQLiteServices.TransactionDatabase
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
