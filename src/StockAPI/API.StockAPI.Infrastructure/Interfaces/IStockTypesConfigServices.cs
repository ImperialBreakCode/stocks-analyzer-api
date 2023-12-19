using API.StockAPI.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Interfaces
{
    public interface IStockTypesConfigServices
    {
        public StockTypesConfigEntry GetStockTypesConfig(string type);
    }
}
