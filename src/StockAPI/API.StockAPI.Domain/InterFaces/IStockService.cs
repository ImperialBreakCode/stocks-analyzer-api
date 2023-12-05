using API.StockAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.InterFaces
{
    public interface IStockService
    {
        public Task<StockData> GetStockFromResponse(string symbol, string? data, string type);
    }
}
