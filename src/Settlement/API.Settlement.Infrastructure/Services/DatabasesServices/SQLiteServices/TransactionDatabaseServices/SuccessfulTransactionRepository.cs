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
    public class SuccessfulTransactionRepository : ISuccessfulTransactionRepository
	{
		private readonly SQLiteConnection _connection;
		private readonly IDateTimeService _dateTimeService;
		public SuccessfulTransactionRepository(SQLiteConnection connection, IDateTimeService dateTimeService)
		{
			_connection = connection;
			_dateTimeService = dateTimeService;
		}

		public void Add(Transaction transaction)
		{
			string commandText = $@"INSERT INTO SuccessfulTransaction
                            (TransactionId, TotalPriceIncludingCommission, Quantity, DateTime, StockName, StockId, UserId, WalletId, UserEmail, IsSale, Message) VALUES 
                            (@TransactionId, @TotalPriceIncludingCommission, @Quantity, @DateTime, @StockName, @StockId, @UserId, @WalletId, @UserEmail, @IsSale, @Message)";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@WalletId", transaction.WalletId);
				command.Parameters.AddWithValue("@UserId", transaction.UserId);
				command.Parameters.AddWithValue("@UserEmail", transaction.UserEmail);
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

		public bool ContainsTransaction(string transactionId)
		{
			bool containsTransaction = false;
			string commandText = $"SELECT COUNT(*) FROM SuccessfulTransaction WHERE TransactionId = @TransactionId";
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

		public void Delete(string transactionId)
		{
			string commandText = $"DELETE FROM SuccessfulTransaction WHERE TransactionId = @TransactionId";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				if (ContainsTransaction(transactionId))
				{
					command.Parameters.AddWithValue("@TransactionId", transactionId);
					command.ExecuteNonQuery();
				}
				_connection.Close();
			}


		}
	}
}
