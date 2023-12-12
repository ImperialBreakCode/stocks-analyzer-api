﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.Models
{
    public class TimedOutCall
    {
        public string? Date { get; set; }
        public string? Query { get; set; }
        public string? Symbol { get; set; }
        public string? Type { get; set; }
    }
}
