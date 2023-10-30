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
        Task<TimeSeriesData> GetCurrentStock(string response, string symbol);

        Task<TimeSeriesData> GetDailyStock(string response, string symbol);

        Task<TimeSeriesData> GetWeeklyStock(string response, string symbol);

        Task<TimeSeriesData> GetMonthlyStock(string response, string symbol);
    }
}
