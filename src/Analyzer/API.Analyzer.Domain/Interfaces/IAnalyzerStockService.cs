using API.Accounts.Application.DTOs.Response;

namespace API.Analyzer.Domain.Interfaces
{
    public interface IAnalyzerStockService
    {
        public Task<ICollection<GetStockResponseDTO>>UserStocksInWallet(string walletId);

        //public Task<decimal> PercentageChange(string symbol);
    }
}
