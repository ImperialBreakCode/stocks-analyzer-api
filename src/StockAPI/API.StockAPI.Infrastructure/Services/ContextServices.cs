using API.StockAPI.Domain.Interfaces;
using API.StockAPI.Domain.Models;
using API.StockAPI.Domain.Utilities;
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
        private readonly IStockTypesConfigServices _configServices;
        private readonly IDateCalculator _dateCalculator;
        private readonly IParametersAssigner _parametersAssigner;
        public ContextServices(DapperContext context,
            IStockTypesConfigServices configServices,
            IDateCalculator dateCalculator,
            IParametersAssigner parametersAssigner)
        {
            _context = context;
            _configServices = configServices;
            _dateCalculator = dateCalculator;
            _parametersAssigner = parametersAssigner;
        }

        public async Task<StockDataDTO> GetStockFromDB(string symbol, string type)
        {
            var stockTypesConfig = _configServices.GetStockTypesConfig(type);

            var table = stockTypesConfig.Table;

            var date = _dateCalculator.GetLastBusinessDay(type);

            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(date))
            {
                return null;
            }

            var query = $"SELECT * FROM {table} WHERE Symbol = @Symbol AND Date = @Date LIMIT 5";

            var parameters = new DynamicParameters();

            parameters.Add("Symbol", symbol, DbType.String);
            parameters.Add("Date", date, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var stock = await connection.QueryFirstOrDefaultAsync<StockDataDTO>(query, parameters);
                connection.Close();
                return stock;
            }
        }

        public async void InsertStockInDB(StockDataDTO data, string type)
        {
            var stockTypesConfig = _configServices.GetStockTypesConfig(type);

            var table = stockTypesConfig.Table;

            if (string.IsNullOrEmpty(table))
            {
                await Console.Out.WriteLineAsync("The data entry is invalid and was not inserted into the database.");
            }

            var query = $"INSERT INTO {table} (Symbol, Date, Open, High, Low, Close, Volume) VALUES (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume)";

            var parameters = _parametersAssigner.AssignParametersToStockDataDTO(data);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
                connection.Close();
            }

            await Console.Out.WriteLineAsync("Record inserted into the database successfully.");
        }
    }
}
