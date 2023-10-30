using API.Analyzer.Domain.DTOs;

namespace API.Analyzer.Domain.Interface
{
    public interface IService
    {
        public bool ProfitablenessAccountCheck(int id, decimal amount);
        public Task<User> UserProfilInfo(int id);
        //public Task<bool> UserPortfolioProfit(string userId);
    }
}
