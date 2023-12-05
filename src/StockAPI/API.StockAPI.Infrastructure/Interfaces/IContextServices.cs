using API.StockAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Interfaces
{
    public interface IContextServices
    {
        public Task<StockData> GetStockFromDB(string symbol, string type);
        public Task<StockData> InsertStockInDB(StockData data, string type);
    }
}
