using API.Analyzer.Domain.DTOs;
namespace API.Analyzer.Domain.Interfaces
{
    public interface IAnalyzerStockService
    {
        public Task<ICollection<GetStockResponseDTO>>UserStocksInWallet(string walletId);

        public Task<decimal?> CalculateInvestmentPercentageGain(string symbol, decimal shareValue);

        public Task<decimal?> GetShareValue(string walletId);
    }
}
