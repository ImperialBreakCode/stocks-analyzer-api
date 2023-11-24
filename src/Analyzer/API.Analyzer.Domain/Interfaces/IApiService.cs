
namespace API.Analyzer.Domain.Interfaces
{

    public interface IApiService
    {
        //public Task<GetWalletResponseDTO> UserProfilInfo(string userName);

        public Task<decimal> CurrentProfitability(string walletId);

        //public Task<decimal> PercentageChange(string symbol);
    }
}