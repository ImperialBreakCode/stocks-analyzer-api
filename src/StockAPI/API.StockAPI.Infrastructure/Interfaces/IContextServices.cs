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
        public Task<StockDataDTO> GetStockFromDB(string symbol, string type);
        public void InsertStockInDB(StockDataDTO data, string type);
    }
}
