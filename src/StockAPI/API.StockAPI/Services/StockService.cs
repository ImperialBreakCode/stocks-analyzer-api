using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;

namespace API.StockAPI.Services
{
    public class StockService : IStockService
    {
        public async Task<StockDataDTO> GetStockFromResponse(string symbol, string response, string type)
        {
            int skipValue = 2;
            if(type == "current")
            {
                skipValue = 1;
            }

            var csvLine = response.Split(Environment.NewLine).Skip(skipValue).ToList().First();

            var result = FromCsvToStockDataDTO(csvLine, symbol);

            return result;
        }

        public async Task<IEnumerable<StockDataDTO>> GetStockList(string symbol, string response, string type)
        {
            int skipValue = 0;
            int entriesCount = 0;
            switch (type)
            {
                case "current":
                    skipValue = 1;
                    entriesCount = 7;
                    break;
                case "daily":
                    skipValue = 2;
                    entriesCount = 7;
                    break;
                case "weekly":
                    skipValue = 2;
                    entriesCount = 3;
                    break;
                case "monthly":
                    skipValue = 2;
                    entriesCount = 3;
                    break;
                default:
                    break;
            }

            var data = response.Split(Environment.NewLine).Skip(skipValue);
            var result = new Collection<StockDataDTO>();

            for (int i = 0; i < entriesCount && i <= data.Count(); i++)
            {
                result.Add(FromCsvToStockDataDTO(data.ElementAt(i), symbol));
            }

            return result;
        }

        public StockDataDTO FromCsvToStockDataDTO(string csvLine, string symbol)
        {
            string[] values = csvLine.Split(',');

            CultureInfo usCulture = CultureInfo.GetCultureInfo("en-US");

            StockDataDTO data = new()
            {
                Symbol = symbol,
                Date = Convert.ToString(values[0]),
                Open = double.Parse(values[1], NumberStyles.Float, usCulture),
                High = double.Parse(values[2], NumberStyles.Float, usCulture),
                Low = double.Parse(values[3], NumberStyles.Float, usCulture),
                Close = double.Parse(values[4], NumberStyles.Float, usCulture),
                Volume = Convert.ToInt32(values[5])
            };

            return data;
        }
    }
}
