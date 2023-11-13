using API.Accounts.Domain.Entities;
using API.Analyzer.Domain.DTOs;

namespace API.Analyzer.Domain.Interfaces
{

    public interface IApiService
    {
        public Task<GetWalletResponseDTO> UserProfilInfo(string userName);
        
       // public Task GetProfitabilityForDate(); //нужна е имплементация в сървъра
        public Task<decimal?> ProfitablenessAccountCheck(string userName, decimal? balance);
    }
}