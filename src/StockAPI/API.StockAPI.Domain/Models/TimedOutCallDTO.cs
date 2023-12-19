using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.Models
{
    public class TimedOutCallDTO
    {
        public string? Date { get; set; }
        public string? Call { get; set; }
        public string? Symbol { get; set; }
        public string? Type { get; set; }
    }
}
