using API.StockAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Interfaces
{
    public interface ITimedOutCallServices
    {
        Task<IEnumerable<TimedOutCallDTO>> GetFailedCallsFromDB();
        Task<IEnumerable<TimedOutCallDTO>> GetFailedCallsFromDBByQuery(string call);
        Task<TimedOutCallDTO> InsertFailedCallInDB(string symbol, string? call, string type);
        Task<StockDataDTO?> RecallFailedCall(TimedOutCallDTO call);
        void DeleteFailedCallInDB(TimedOutCallDTO call);
    }
}
