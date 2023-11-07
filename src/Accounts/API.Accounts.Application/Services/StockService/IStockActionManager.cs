using API.Accounts.Application.DTOs.ExternaDTOs;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockActionManager
    {
        Task ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
        Task ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
    }
}
