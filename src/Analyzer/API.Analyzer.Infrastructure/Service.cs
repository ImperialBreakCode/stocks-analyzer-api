using API.Analyzer.Domain.DTOs;
using API.Analyzer.Domain.Interface;

namespace API.Analyzer
{
    public class Service// : IService
    {
       

        public bool ProfitablenessAccountCheck(int id, decimal amount)
        {
            return true;
        }

        public Task<User> UserProfilInfo(int id)
        {
            throw new NotImplementedException();
        }

        //public Task<bool> UserPortfolioProfit(string userId);
        //{
        //    return true;
        //}
    }
}
