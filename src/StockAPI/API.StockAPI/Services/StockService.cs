using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using Newtonsoft.Json;

namespace API.StockAPI.Services
{
    public class StockService : IStockService
    {
        public async Task<StockData> GetStockFromResponse(string symbol, string response, string type)
        {
            var csvLine = response.Split(Environment.NewLine).Skip(1).ToList().First();

            var result = FromCsv(csvLine, symbol);

            return result;
        }

        public StockData FromCsv(string csvLine, string symbol)
        {
            string[] values = csvLine.Split(',');

            StockData data = new()
            {
                Symbol = symbol,
                Date = Convert.ToString(values[0]),
                Open = Convert.ToDouble(values[1]),
                High = Convert.ToDouble(values[2]),
                Low = Convert.ToDouble(values[3]),
                Close = Convert.ToDouble(values[4]),
                Volume = Convert.ToInt32(values[5])
            };

            return data;
        }
    }
}
