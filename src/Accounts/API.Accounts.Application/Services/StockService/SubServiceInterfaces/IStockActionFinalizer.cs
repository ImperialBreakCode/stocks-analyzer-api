namespace API.Accounts.Application.Services.StockService.SubServiceInterfaces
{
    public interface IStockActionFinalizer
    {
        Task<string> ConfirmSales(string walletId);
        Task<string> ConfirmPurchase(string walletId);
    }
}
