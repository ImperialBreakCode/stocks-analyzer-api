using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using API.StockAPI.Infrastructure.Context;
using API.StockAPI.Infrastructure.Interfaces;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Services
{
    public class TimedOutCallServices : ITimedOutCallServices
    {
        private readonly DapperContext _context;
        private readonly IExternalRequestService _externalRequestService;
        private readonly IStockService _stockService;
        private readonly IContextServices _contextServices;
        public TimedOutCallServices(DapperContext context,
            IExternalRequestService externalRequestService,
            IStockService stockService,
            IContextServices contextServices)
        {
            _context = context;
            _externalRequestService = externalRequestService;
            _stockService = stockService;
            _contextServices = contextServices;
        }

        public async Task<IEnumerable<TimedOutCall>> GetFailedCallsFromDB()
        {
            var query = $"SELECT * FROM TimedOutCalls";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var calls = await connection.QueryAsync<TimedOutCall>(query);
                connection.Close();
                return calls;
            }
        }
        public async Task<IEnumerable<TimedOutCall>> GetFailedCallsFromDBByQuery(string call)
        {
            var query = $"SELECT * FROM TimedOutCalls WHERE Query = @Query";

            var parameters = new DynamicParameters();

            parameters.Add("Query", call, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var calls = await connection.QueryAsync<TimedOutCall>(query, parameters);
                connection.Close();
                return calls;
            }
        }

        public async Task<TimedOutCall> InsertFailedCallInDB(string symbol, string? call, string type)
        {
            if (!GetFailedCallsFromDBByQuery(call).Result.Any())
            {
                return null;
            }
            var query = $"INSERT INTO TimedOutCalls (Date, Symbol, Query, Type) VALUES (@Date, @Symbol, @Query, @Type)";

            var data = new TimedOutCall()
            {
                Date = DateTime.Now.ToString(),
                Symbol = symbol,
                Query = call,
                Type = type
            };

            var parameters = AssignParametersToCall(data);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
                connection.Close();
            }

            return data;
        }

        public async Task<StockData?> RecallFailedCall(TimedOutCall call)
        {
            if (!GetFailedCallsFromDBByQuery(call.Query).Result.Any())
            {
                return null;
            }

            var response = await _externalRequestService.ExecuteQuery(call.Symbol, call.Query, call.Type);

            var data = await _externalRequestService.GetDataFromQuery(response);
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = await _stockService.GetStockFromResponse(call.Symbol, data, call.Type);
            if (result is null)
            {
                return null;
            }

            await _contextServices.InsertStockInDB(result, call.Type);

            return result;
        }

        public async void DeleteFailedCallInDB(TimedOutCall call)
        {
            var query = $"DELETE FROM TimedOutCalls WHERE Query = @Call AND Date = @Date";

            var data = new TimedOutCall()
            {
                Date = DateTime.Now.ToString(),
                Symbol = call.Symbol,
                Query = call.Query,
                Type = call.Type
            };

            var parameters = AssignParametersToCall(data);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
                connection.Close();
            }
        }

        private DynamicParameters AssignParametersToCall(TimedOutCall data)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Date", data.Date, DbType.String);
            parameters.Add("Symbol", data.Symbol, DbType.String);
            parameters.Add("Call", data.Query, DbType.String);
            parameters.Add("Type", data.Type, DbType.String);

            return parameters;
        }
    }
}
