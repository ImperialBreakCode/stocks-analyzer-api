using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API.Gateway.Infrastructure.Contexts
{
	public class SQLiteContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public SQLiteContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("Default");
		}

		public IDbConnection CreateConnection()
			=> new SqliteConnection(_connectionString);
	}
}
