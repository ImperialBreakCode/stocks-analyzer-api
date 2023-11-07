using API.Accounts.Domain.Entities;
using API.Analyzer.Domain.DTOs;

namespace API.Analyzer.Domain.Interfaces
{

    public interface IApiService
    {
        public Task<Wallet> UserProfilInfo(string userId);
        public bool GetAction(string userId);
        public Task<decimal?> ProfitablenessAccountCheck(string userId, decimal balance);
    }
}