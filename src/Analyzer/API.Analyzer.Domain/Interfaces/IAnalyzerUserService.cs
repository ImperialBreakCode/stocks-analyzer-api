using API.Accounts.Application.DTOs.Response;
namespace API.Analyzer.Domain.Interfaces
{

    public interface IAnalyzerUserService
    {
        public Task<GetWalletResponseDTO> PortfolioSummary(string walletId);

        public Task<decimal> CurrentProfitability(string walletId);
       
    }
}