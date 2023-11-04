using API.Accounts.Application.DTOs.Request;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockService
    {
        Task<string> AddForPurchase(StockActionDTO stockActionDTO);
        string AddForSale(StockActionDTO stockActionDTO);
        string ConfirmSales(string walletId);
        string ConfirmPurchase(string walletId);
    }
}
