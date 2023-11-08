using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockActionManager
    {
        Task<FinalizeStockResponseDTO> ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
        Task<FinalizeStockResponseDTO> ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
    }
}
