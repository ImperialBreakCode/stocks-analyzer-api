using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Analyzer.Domain.DTOs
{
    public class GetStockResponseDTO
    {
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public string WalletId { get; set; }
    }
}
