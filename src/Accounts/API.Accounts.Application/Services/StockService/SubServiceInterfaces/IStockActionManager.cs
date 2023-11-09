using API.Accounts.Application.DTOs.Request;

namespace API.Accounts.Application.Services.StockService.SubServiceInterfaces
{
    public interface IStockActionManager
    {
        Task<string> AddForPurchase(StockActionDTO stockActionDTO);
        string AddForSale(StockActionDTO stockActionDTO);
    }
}
