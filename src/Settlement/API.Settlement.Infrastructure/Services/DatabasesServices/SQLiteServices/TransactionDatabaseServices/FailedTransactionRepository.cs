using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.SQLiteServices
{
    public class FailedTransactionRepository : IFailedTransactionRepository
	{
		private readonly SQLiteConnection _connection;
		private readonly IDateTimeService _dateTimeService;
		public FailedTransactionRepository(SQLiteConnection connection, IDateTimeService dateTimeService)
		{
			_connection = connection;
			_dateTimeService = dateTimeService;
		}
		public void Add(Transaction transaction)
		{
			string commandText = $@"INSERT INTO FailedTransaction
                            (TransactionId, TotalPriceIncludingCommission, Quantity, DateTime, StockName, StockId, UserId, WalletId, IsSale, Message) VALUES 
                            (@TransactionId, @TotalPriceIncludingCommission, @Quantity, @DateTime, @StockName, @StockId, @UserId, @WalletId, @IsSale, @Message)";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@WalletId", transaction.WalletId);
				command.Parameters.AddWithValue("@UserId", transaction.UserId);
				command.Parameters.AddWithValue("@IsSale", transaction.IsSale);
				command.Parameters.AddWithValue("TransactionId", transaction.TransactionId);
				command.Parameters.AddWithValue("TotalPriceIncludingCommission", transaction.TotalPriceIncludingCommission);
				command.Parameters.AddWithValue("Quantity", transaction.Quantity);
				command.Parameters.AddWithValue("DateTime", _dateTimeService.UtcNow);
				command.Parameters.AddWithValue("StockName", transaction.StockName);
				command.Parameters.AddWithValue("StockId", transaction.StockId);
				command.Parameters.AddWithValue("Message", transaction.Message);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public void Delete(string transactionId)
		{
			string commandText = $"DELETE FROM FailedTransaction WHERE TransactionId = @TransactionId";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

					command.Parameters.AddWithValue("@TransactionId", transactionId);
					command.ExecuteNonQuery();
				
				_connection.Close();
			}


		}

		public IEnumerable<Transaction> GetAll()
		{
			string commandText = "SELECT * FROM FailedTransaction";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					var failedTransactions = new List<Transaction>();

					while (reader.Read())
					{
						var failedTransaction = new Transaction
						{
							TransactionId = Convert.ToString(reader["TransactionId"]),
							WalletId = Convert.ToString(reader["WalletId"]),
							UserId = Convert.ToString(reader["UserId"]),
							IsSale = Convert.ToBoolean(reader["IsSale"]),
							Message = Convert.ToString(reader["Message"]),
							StockId = Convert.ToString(reader["StockId"]),
							StockName = Convert.ToString(reader["StockName"]),
							Quantity = Convert.ToInt32(reader["Quantity"]),
							TotalPriceIncludingCommission = Convert.ToDecimal(reader["TotalPriceIncludingCommission"]),
						};

						failedTransactions.Add(failedTransaction);
					}
					_connection.Close();
					return failedTransactions;
				}
			}
		}
		public bool ContainsTransaction(string transactionId)
		{
			bool containsTransaction = false;
			string commandText = $"SELECT COUNT(*) FROM FailedTransaction WHERE TransactionId = @TransactionId";
			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				command.Parameters.AddWithValue("@TransactionId", transactionId);

				int count = Convert.ToInt32(command.ExecuteScalar());
				containsTransaction = (count > 0);
				_connection.Close();

			}

			return containsTransaction;
		}
	}
}
