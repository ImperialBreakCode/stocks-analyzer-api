using Dapper;
using Microsoft.Data.Sqlite;

namespace API.StockAPI.Infrastructure.Helpers
{
    public class DatabaseHelper
    {
        private const string CREATE_WEEKLY = "CREATE TABLE IF NOT EXISTS Weekly (Symbol TEXT , Date TEXT, Open REAL, Low REAL, High REAL, Close REAL, Volume INTEGER)";
        private const string CREATE_MONTHLY = "CREATE TABLE IF NOT EXISTS Monthly (Symbol TEXT , Date TEXT, Open REAL, Low REAL, High REAL, Close REAL, Volume INTEGER)";
        private const string CREATE_TIMED_OUT_CALLS = "CREATE TABLE IF NOT EXISTS TimedOutCalls (Date TEXT NOT NULL, Symbol TEXT NOT NULL, Call TEXT NOT NULL, Type TEXT NOT NULL)";
        public static void EnsureDatabaseExists(string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                CreateDatabaseSchema(connection);

                connection.Close();
            }
        }

        private static async void CreateDatabaseSchema(SqliteConnection connection)
        {
            // Create tables and define schema here
            using (connection)
            {
                await connection.ExecuteAsync(CREATE_WEEKLY);
                await connection.ExecuteAsync(CREATE_MONTHLY);
                await connection.ExecuteAsync(CREATE_TIMED_OUT_CALLS);
            }
        }
    }
}
