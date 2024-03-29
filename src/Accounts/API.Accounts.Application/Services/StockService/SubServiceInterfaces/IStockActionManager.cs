﻿using API.Accounts.Application.DTOs.Request;

namespace API.Accounts.Application.Services.StockService.SubServiceInterfaces
{
    public interface IStockActionManager
    {
        Task<string> AddForPurchase(StockActionDTO stockActionDTO, string username);
        Task<string> AddForSale(StockActionDTO stockActionDTO, string username);
    }
}
