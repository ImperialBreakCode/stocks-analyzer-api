using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using System.Data.SQLite;

namespace API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices
{
	public class FailedTransactionRepository : IFailedTransactionRepository
	{
		private readonly SQLiteConnection _connection;
		private readonly IDateTimeHelper _dateTimeHelper;

		public FailedTransactionRepository(SQLiteConnection connection,
										   IDateTimeHelper dateTimeHelper)
		{
			_connection = connection;
			_dateTimeHelper = dateTimeHelper;
		}

		public void Add(Transaction transaction)
		{
			string commandText = $@"INSERT INTO FailedTransaction
                            (TransactionId, TotalPriceIncludingCommission, Quantity, DateTime, StockName, StockId, UserId, WalletId, UserEmail, IsSale, UserRank, Message) VALUES 
                            (@TransactionId, @TotalPriceIncludingCommission, @Quantity, @DateTime, @StockName, @StockId, @UserId, @WalletId, @UserEmail, @IsSale, @UserRank, @Message)";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();
				command.Parameters.AddWithValue("@WalletId", transaction.WalletId);
				command.Parameters.AddWithValue("@UserId", transaction.UserId);
				command.Parameters.AddWithValue("@UserEmail", transaction.UserEmail);
				command.Parameters.AddWithValue("@IsSale", transaction.IsSale);
				command.Parameters.AddWithValue("@UserRank", transaction.UserRank);
				command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
				command.Parameters.AddWithValue("@TotalPriceIncludingCommission", transaction.TotalPriceIncludingCommission);
				command.Parameters.AddWithValue("@Quantity", transaction.Quantity);
				command.Parameters.AddWithValue("@DateTime", _dateTimeHelper.UtcNow);
				command.Parameters.AddWithValue("@StockName", transaction.StockName);
				command.Parameters.AddWithValue("@StockId", transaction.StockId);
				command.Parameters.AddWithValue("@Message", transaction.Message);
				command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public Transaction Delete(string transactionId)
		{
			var transaction = GetById(transactionId);
			string commandText = $"DELETE FROM FailedTransaction WHERE TransactionId = @TransactionId";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				command.Parameters.AddWithValue("@TransactionId", transactionId);
				command.ExecuteNonQuery();

				_connection.Close();
			}
			return transaction;
		}

		public Transaction GetById(string transactionId)
		{
			string commandText = "SELECT * FROM FailedTransaction WHERE TransactionId = @TransactionId";
			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				command.Parameters.AddWithValue("@TransactionId", transactionId);
				var transaction = new Transaction();
				_connection.Open();
				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						transaction = new Transaction
						{
							TransactionId = Convert.ToString(reader["TransactionId"]),
							WalletId = Convert.ToString(reader["WalletId"]),
							UserId = Convert.ToString(reader["UserId"]),
							UserEmail = Convert.ToString(reader["UserEmail"]),
							IsSale = Convert.ToBoolean(reader["IsSale"]),
							UserRank = (UserRank)Convert.ToInt32(reader["UserRank"]),
							Message = Convert.ToString(reader["Message"]),
							StockId = Convert.ToString(reader["StockId"]),
							StockName = Convert.ToString(reader["StockName"]),
							Quantity = Convert.ToInt32(reader["Quantity"]),
							TotalPriceIncludingCommission = Convert.ToDecimal(reader["TotalPriceIncludingCommission"]),
						};
						break;
					}
				}
				_connection.Close();
				return transaction;
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
							UserEmail = Convert.ToString(reader["UserEmail"]),
							IsSale = Convert.ToBoolean(reader["IsSale"]),
							UserRank = (UserRank)Convert.ToInt32(reader["UserRank"]),
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
			try
			{
				using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
				{
					_connection.Open();

					command.Parameters.AddWithValue("@TransactionId", transactionId);

					int count = Convert.ToInt32(command.ExecuteScalar());
					containsTransaction = count > 0;
					_connection.Close();

				}
				return containsTransaction;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return false;

		}

		public bool ContainsTransactionsWithWalletId(string walletId)
		{
			bool containsTransactionsWithWalletId = false;
			string commandText = $"SELECT COUNT(*) FROM FailedTransaction WHERE WalletId = @WalletId";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				command.Parameters.AddWithValue("@WalletId", walletId);

				int count = Convert.ToInt32(command.ExecuteScalar());
				containsTransactionsWithWalletId = count > 0;
				_connection.Close();

			}
			return containsTransactionsWithWalletId;
		}

		public void DeleteTransactionsWithWalletId(string walletId)
		{
			string commandText = $"DELETE FROM FailedTransaction WHERE WalletId = @WalletId";

			using (SQLiteCommand command = new SQLiteCommand(commandText, _connection))
			{
				_connection.Open();

				command.Parameters.AddWithValue("@WalletId", walletId);
				command.ExecuteNonQuery();

				_connection.Close();
			}
		}

	}
}
