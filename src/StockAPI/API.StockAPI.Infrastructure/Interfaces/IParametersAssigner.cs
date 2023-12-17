using API.StockAPI.Domain.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Interfaces
{
    public interface IParametersAssigner
    {
        public DynamicParameters AssignParametersToStockDataDTO(StockDataDTO data);
    }
}
