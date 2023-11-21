﻿using API.Accounts.Domain.Entities;
using API.Analyzer.Domain.DTOs;

namespace API.Analyzer.Domain.Interfaces
{

    public interface IApiService
    {
        public Task<GetWalletResponseDTO> UserProfilInfo(string userName);
        
        public Task<decimal?> CurrentProfitability(string userName);

        public Task<decimal> PercentageChange(string symbol);
    }
}