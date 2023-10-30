using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using Newtonsoft.Json;

namespace API.StockAPI.Services
{
    public class StockService : IStockService
    {
        public async Task<TimeSeriesData> GetCurrentStock(string response, string symbol)
        {
            CurrentData? data = JsonConvert.DeserializeObject<CurrentData>(response);

            var timeSeriesData = data.TimeSeries;
            timeSeriesData.First().Value.Symbol = symbol;
            timeSeriesData.First().Value.Date = timeSeriesData.First().Key;

            var result = timeSeriesData.First().Value;
            return result;
        }

        public async Task<TimeSeriesData> GetDailyStock(string response, string symbol)
        {
            DailyData? data = JsonConvert.DeserializeObject<DailyData>(response);

            var timeSeriesData = data.TimeSeries;
            timeSeriesData.First().Value.Symbol = symbol;
            timeSeriesData.First().Value.Date = timeSeriesData.First().Key;

            var result = timeSeriesData.First().Value;
            return result;
        }

        public async Task<TimeSeriesData> GetWeeklyStock(string response, string symbol)
        {
            throw new NotImplementedException();
        }

        public async Task<TimeSeriesData> GetMonthlyStock(string response, string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
