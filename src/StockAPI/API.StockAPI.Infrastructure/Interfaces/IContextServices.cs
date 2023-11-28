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
        public Task<StockData> Get(string symbol, string type);
        public Task<StockData> Create(StockData data, string type);
    }
}
