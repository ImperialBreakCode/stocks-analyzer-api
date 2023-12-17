using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.Interfaces
{
    public interface IDateCalculator 
    {
        public string GetLastBusinessDay(string type);
    }
}
