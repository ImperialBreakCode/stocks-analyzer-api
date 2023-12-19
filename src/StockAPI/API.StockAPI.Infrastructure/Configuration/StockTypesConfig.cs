using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Configuration
{
    public class StockTypesConfig
    {
        public Dictionary<string, StockTypesConfigEntry> Types { get; set; }
    }
}
