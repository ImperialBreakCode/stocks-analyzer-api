using API.StockAPI.Infrastructure.Configuration;
using API.StockAPI.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Services
{
    public class StockTypesConfigServices : IStockTypesConfigServices
    {
        private readonly IOptionsSnapshot<StockTypesConfig> _stockTypeConfig;
        public StockTypesConfigServices(IOptionsSnapshot<StockTypesConfig> stockTypeConfig)
        {
            _stockTypeConfig = stockTypeConfig;
        }

        public StockTypesConfigEntry GetStockTypesConfig(string type)
        {
            if (_stockTypeConfig.Value.Types.TryGetValue(type, out var configEntry))
            {
                return new StockTypesConfigEntry
                {
                    SkipValue = configEntry.SkipValue,
                    EntriesCount = configEntry.EntriesCount,
                    Table = configEntry.Table
                };
            }
            else
            {
                throw new InvalidOperationException($"Configuration not found for type: {type}");
            }
        }
    }
}
