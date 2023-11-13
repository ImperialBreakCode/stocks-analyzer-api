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
        public Task<StockData> GetWeeklyStock(string symbol);
        public Task<StockData> GetMonthlyStock(string symbol);
        public Task CreateWeeklyStock(StockData data);
        public Task CreateMonthlyStock(StockData data);
    }
}
