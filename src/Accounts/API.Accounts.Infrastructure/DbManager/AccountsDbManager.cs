using API.Accounts.Domain.Interfaces.DbManager;
using API.Accounts.Infrastructure.SqlScripts;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Accounts.Infrastructure.DbManager
{
    public class AccountsDbManager : IAccountsDbManager
    {
        private const string _tableColumnsCountQuery = @"SELECT Count(*)
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}';";

        private readonly SqlConnection _connection;

        public AccountsDbManager()
        {
            _connection = new SqlConnection();
        }

        public void EnsureDatabaseTables(string connectionString)
        {
            _connection.ConnectionString = connectionString;
            _connection.Open();

            foreach (var creationScript in TableCreationScripts.AllCreationQueries)
            {
                CreateTableIfItDoesntExist(creationScript.Key, creationScript.Value);
            }

            _connection.Close();
        }

        private void CreateTableIfItDoesntExist(string tableName, string creationQuery)
        {
            SqlCommand sqlCommand = CreateCommand(string.Format(_tableColumnsCountQuery, tableName));
            if ((int)sqlCommand.ExecuteScalar() == 0)
            {
                sqlCommand = CreateCommand(creationQuery);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private SqlCommand CreateCommand(string query)
        {
            SqlCommand sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;

            return sqlCommand;
        }
    }
}
