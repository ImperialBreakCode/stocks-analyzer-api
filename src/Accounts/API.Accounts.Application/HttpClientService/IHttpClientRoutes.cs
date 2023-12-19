namespace API.Accounts.Application.HttpClientService
{
    public interface IHttpClientRoutes
    {
        string FinalizeStockActionRoute { get; }
        string GetCurrentStockInfoRoute(string stockName);
    }
}
