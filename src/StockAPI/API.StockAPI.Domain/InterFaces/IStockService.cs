﻿using API.StockAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.InterFaces
{
    public interface IStockService
    {
        public Task<StockDataDTO> GetStockFromResponse(string symbol, string? data, string type);
        public Task<IEnumerable<StockDataDTO>> GetStockList(string symbol, string response, string type);
    }
}
