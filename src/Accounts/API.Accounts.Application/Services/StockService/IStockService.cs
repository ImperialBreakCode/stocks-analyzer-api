using API.Accounts.Application.DTOs.Request;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockService
    {
        Task<string> AddForPurchase(StockActionDTO stockActionDTO);
        string AddForSale(StockActionDTO stockActionDTO);
        Task<string> ConfirmSales(string walletId);
        Task<string> ConfirmPurchase(string walletId);
    }
}
