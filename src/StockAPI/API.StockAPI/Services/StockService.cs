using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Models;
using API.StockAPI.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;

namespace API.StockAPI.Services
{
    public class StockService : IStockService
    {
        private readonly IStockTypesConfigServices _configServices;
        public StockService(IStockTypesConfigServices configServices)
        {
            _configServices = configServices;
        }
        public async Task<StockDataDTO> GetStockFromResponse(string symbol, string response, string type)
        {
            var stockTypesConfig = _configServices.GetStockTypesConfig(type);

            int skipValue = stockTypesConfig.SkipValue;

            var csvLine = response.Split(Environment.NewLine).Skip(skipValue).ToList().First();

            var result = FromCsvToStockDataDTO(csvLine, symbol);

            return result;
        }

        public async Task<IEnumerable<StockDataDTO>> GetStockList(string symbol, string response, string type)
        {
            var stockTypesConfig = _configServices.GetStockTypesConfig(type);

            int skipValue = stockTypesConfig.SkipValue;
            int entriesCount = stockTypesConfig.EntriesCount;

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
