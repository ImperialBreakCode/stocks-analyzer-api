namespace API.Accounts.Application.Data.StocksData
{
    public interface IStocksData
    {
        Task<decimal> GetCurrentStockPrice(string stockName);
    }
}
