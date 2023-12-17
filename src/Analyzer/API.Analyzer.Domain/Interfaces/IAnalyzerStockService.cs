using API.Analyzer.Domain.DTOs;
namespace API.Analyzer.Domain.Interfaces
{
    public interface IAnalyzerStockService
    {
        public Task<ICollection<GetStockResponseDTO>>UserStocksInWallet(string walletId);

        public Task<decimal?> GetCurrentProfitability(string username,string symbol,string type);

        public Task<StockData> GetStockData(string symbol, string type);

        //public Task<decimal?> CalculateInvestmentPercentageGain(string symbol,string type, decimal shareValue);

        public Task<List<decimal>> PersentageChange(string username, string symbol, string type);

        public Task<decimal?> GetShareValue(string walletId);

        public Task<decimal?> CalculateAverageProfitability(string username, string symbol, string type);

        //public Task<List<string?>> DailyProfitabilityChanges(string symbol, string type);
    }
}
