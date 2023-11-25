using API.Accounts.Application.DTOs.Response;
namespace API.Analyzer.Domain.Interfaces
{

    public interface IApiService
    {
<<<<<<< HEAD
        public Task<GetWalletResponseDTO> PortfolioSummary(string userName);

        public Task<decimal> CurrentProfitability(string walletId);

        //public Task<decimal> PercentageChange(string symbol);
=======
        public Task<GetWalletResponseDTO> UserProfilInfo(string userName);
        
        public Task<decimal?> CurrentProfitability(string userName);

        public Task<decimal> PercentageChange(string symbol);
>>>>>>> 682d569c34552675f8606ad5b870853480b56f05
    }
}