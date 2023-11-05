namespace API.Accounts.Application.Services.HttpService
{
    public interface IHttpClientRoutes
    {
        string FinalizeStockActionRoute { get; }
        string GetCurrentStockInfoRoute(string stockName);
    }
}
