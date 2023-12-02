using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
