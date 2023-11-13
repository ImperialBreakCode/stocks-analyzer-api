using API.Accounts.Application.DTOs.ExternalRequestDTOs;
using API.Accounts.Application.DTOs.ExternalResponseDTOs;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService.SubServiceInterfaces
{
    public interface IStockActionExecuter
    {
        Task<FinalizeStockResponseDTO> ExecutePurchase(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
        Task<FinalizeStockResponseDTO> ExecuteSell(FinalizeStockActionDTO finalizeDto, ICollection<Stock> stocks);
    }
}
