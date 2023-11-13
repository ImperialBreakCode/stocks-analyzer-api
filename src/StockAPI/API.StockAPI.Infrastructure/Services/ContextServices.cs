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

        public async Task<StockData> GetWeeklyStock(string symbol)
        {
            var query = "SELECT TOP 1 * FROM Weekly WHERE Symbol = @Symbol";

            using (var connection = _context.CreateConnection())
            {
                var stock = await connection.QuerySingleOrDefaultAsync<StockData>(query);
                return stock;
            }
        }

        public async Task<StockData> GetMonthlyStock(string symbol)
        {
            var query = "SELECT TOP 1 * FROM Monthly WHERE Symbol = @Symbol";

            using (var connection = _context.CreateConnection())
            {
                var stock = await connection.QuerySingleOrDefaultAsync<StockData>(query);
                return stock;
            }
        }

        public async Task CreateWeeklyStock(StockData data)
        {
            var query = "INSERT INTO Weekly (Symbol, Date, Open, High, Low, Close, Volume) VALUES (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume)";

            var parameters = AssignParameters(data);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task CreateMonthlyStock(StockData data)
        {
            var query = "INSERT INTO Weekly (Symbol, Date, Open, High, Low, Close, Volume) VALUES (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume)";

            var parameters = AssignParameters(data);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
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
