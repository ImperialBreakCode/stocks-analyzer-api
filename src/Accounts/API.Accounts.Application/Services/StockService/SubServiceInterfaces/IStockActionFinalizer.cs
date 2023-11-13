namespace API.Accounts.Application.Services.StockService.SubServiceInterfaces
{
    public interface IStockActionFinalizer
    {
        Task<string> ConfirmSales(string username);
        Task<string> ConfirmPurchase(string username);
    }
}
