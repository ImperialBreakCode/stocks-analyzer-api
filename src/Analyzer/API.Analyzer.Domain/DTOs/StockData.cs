﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Analyzer.Domain.DTOs
{
    public class StockData
    {
        public string? Symbol { get; set; }
        public string? Date { get; set; }
        public double Open { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }

    }
}
