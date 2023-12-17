using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Domain.Interfaces.Services
{
    public interface IAnalyzerService
    {
        Task<IActionResult> CalculateAverageProfitability(string username, string symbol, string type);
        Task<IActionResult> CurrentBalanceInWallet(string walletId);
        Task<IActionResult> CurrentProfitability(string username, string symbol, string type);
        Task<IActionResult> GetUserStocksInWallet(string walletId);
        Task<IActionResult> PercentageChange(string username, string symbol, string type);
        Task<IActionResult> PortfolioSummary(string walletId);
    }
}