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

        public async Task<StockData> GetStockFromDB(string symbol, string type)
        {
            var date = "";
            var table = "";
            switch (type)
            {
                case "weekly":
                    table = "Weekly";
                    date = GetLastBusinessDayOfLastWeek();
                    break;
                case "monthly":
                    table = "Monthly";
                    date = GetLastBusinessDayOfLastMonth();
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

        public async Task<StockData> InsertStockInDB(StockData data, string type)
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

        private string GetLastBusinessDayOfLastWeek()
        {
            var currentDate = DateTime.Now;

            DateTime lastWeekEndDate = currentDate.AddDays(-((int)currentDate.DayOfWeek + 1));

            DateTime lastFriday = lastWeekEndDate.AddDays(-1);

            return lastFriday.ToString("yyyy-MM-dd");
        }
        private string GetLastBusinessDayOfLastMonth()
        {
            var lastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month-1));

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                lastDayOfMonth = lastDayOfMonth.AddDays(-2);
            }   
            else if (lastDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
            {
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);
            }

            return lastDayOfMonth.ToString("yyyy-MM-dd");
        }
    }
}
