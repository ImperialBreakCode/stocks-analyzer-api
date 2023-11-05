using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockService
    {
        GetStockResponseDTO? GetStockById(string stockId);
        Task<string> AddForPurchase(StockActionDTO stockActionDTO);
        string AddForSale(StockActionDTO stockActionDTO);
        Task<string> ConfirmSales(string walletId);
        Task<string> ConfirmPurchase(string walletId);
    }
}
