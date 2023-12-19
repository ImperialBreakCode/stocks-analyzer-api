using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;

namespace API.Accounts.Application.Services.StockService
{
    public interface IStockService
    {
        IStockActionFinalizer ActionFinalizer { get; }
        IStockActionManager ActionManager { get; }

        GetStockResponseDTO? GetStockById(string stockId);
        ICollection<GetStockResponseDTO>? GetStocksByWalletId(string walletId);
    }
}
