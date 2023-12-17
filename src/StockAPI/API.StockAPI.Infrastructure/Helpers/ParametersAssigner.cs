using API.StockAPI.Domain.Models;
using API.StockAPI.Infrastructure.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Helpers
{
    public class ParametersAssigner : IParametersAssigner
    {
        public DynamicParameters AssignParametersToStockDataDTO(StockDataDTO data)
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
