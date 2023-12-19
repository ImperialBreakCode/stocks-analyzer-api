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

        public async Task<IEnumerable<TimedOutCallDTO>> GetFailedCallsFromDB()
        {
            var query = $"SELECT * FROM TimedOutCalls";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var calls = await connection.QueryAsync<TimedOutCallDTO>(query);
                connection.Close();
                return calls;
            }
        }
        public async Task<IEnumerable<TimedOutCallDTO>> GetFailedCallsFromDBByQuery(string call)
        {
            var query = $"SELECT * FROM TimedOutCalls WHERE Call = @Call";

            var parameters = new DynamicParameters();

            parameters.Add("Call", call, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var calls = await connection.QueryAsync<TimedOutCallDTO>(query, parameters);
                connection.Close();
                return calls;
            }
        }

        public async Task<TimedOutCallDTO> InsertFailedCallInDB(string symbol, string? call, string type)
        {
            var calls = await GetFailedCallsFromDBByQuery(call);
            var check = calls.Any();
            if (check is not false)
            {
                return null;
            }
            var query = $"INSERT INTO TimedOutCalls (Date, Symbol, Call, Type) VALUES (@Date, @Symbol, @Call, @Type)";

            var data = new TimedOutCallDTO()
            {
                Date = DateTime.Now.ToString(("MM/dd/yyyy HH")),
                Symbol = symbol,
                Call = call,
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

        public async Task<StockDataDTO?> RecallFailedCall(TimedOutCallDTO call)
        {
            if (!GetFailedCallsFromDBByQuery(call.Call).Result.Any())
            {
                return null;
            }

            var response = await _externalRequestService.ExecuteQuery(call.Symbol, call.Call, call.Type);

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

            _contextServices.InsertStockInDB(result, call.Type);

            return result;
        }

        public async void DeleteFailedCallInDB(TimedOutCallDTO call)
        {
            var query = $"DELETE FROM TimedOutCalls WHERE Call = @Call AND Date = @Date";

            var data = new TimedOutCallDTO()
            {
                Date = DateTime.Now.ToString(),
                Symbol = call.Symbol,
                Call = call.Call,
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

        private DynamicParameters AssignParametersToCall(TimedOutCallDTO data)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Date", data.Date, DbType.String);
            parameters.Add("Symbol", data.Symbol, DbType.String);
            parameters.Add("Call", data.Call, DbType.String);
            parameters.Add("Type", data.Type, DbType.String);

            return parameters;
        }
    }
}
