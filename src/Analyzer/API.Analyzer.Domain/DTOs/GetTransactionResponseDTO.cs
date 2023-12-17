using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Analyzer.Domain.DTOs
{
    public class GetTransactionResponseDTO
    {
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string StockId { get; set; }
        public string Walletid { get; set; }
        public string StockName { get; set; }
    }
}
