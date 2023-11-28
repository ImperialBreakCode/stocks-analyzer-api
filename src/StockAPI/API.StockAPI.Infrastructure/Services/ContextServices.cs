using API.StockAPI.Domain.Models;
using API.StockAPI.Infrastructure.Context;
using API.StockAPI.Infrastructure.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Services
{
    public class ContextServices : IContextServices
    {
        private readonly DapperContext _context;
        public ContextServices(DapperContext context)
        {
            _context = context;
        }

        public async Task<StockData> Get(string symbol, string type)
        {
            var date = DateTime.Now.AddDays(-21).ToString("yyyy-MM-dd") + " 00:00:00";

            var table = "";
            switch (type)
            {
                case "weekly":
                    table = "Weekly";
                    break;
                case "monthly":
                    table = "Monthly";
                    break;
                default:
                    return null;
            }

            var query = $"SELECT * FROM {table} WHERE Symbol = @Symbol AND Date = @Date LIMIT 5";

            var parameters = new DynamicParameters();

            parameters.Add("Symbol", symbol, DbType.String);
            parameters.Add("Date", date, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var stock = await connection.QueryFirstOrDefaultAsync<StockData>(query, parameters);
                connection.Close();
                return stock;
            }
        }

        public async Task<StockData> Create(StockData data, string type)
        {
            var table = "";
            switch (type)
            {
                case "weekly":
                    table = "Weekly";
                    break;
                case "monthly":
                    table = "Monthly";
                    break;
                default:
                    return null;
            }

            var query = $"INSERT INTO {table} (Symbol, Date, Open, High, Low, Close, Volume) VALUES (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume)";

            var parameters = AssignParameters(data);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
                connection.Close();
            }

            return data;
        }

        public DynamicParameters AssignParameters(StockData data)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Symbol", data.Symbol, DbType.String);
            parameters.Add("Date", data.Date, DbType.String);
            parameters.Add("Open", data.Open, DbType.Double);
            parameters.Add("High", data.High, DbType.Double);
            parameters.Add("Low", data.Low, DbType.Double);
            parameters.Add("Close", data.Close, DbType.Double);
            parameters.Add("Volume", data.Volume, DbType.Int32);

            return parameters;
        }
    }
}
