using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Configuration
{
    public class StockTypesConfigEntry
    {
        public int SkipValue { get; set; }
        public int EntriesCount { get; set; }
        public string Table {  get; set; }
    }
}
