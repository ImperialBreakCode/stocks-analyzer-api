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
        Task<IEnumerable<TimedOutCall>> GetFailedCallsFromDB();
        Task<IEnumerable<TimedOutCall>> GetFailedCallsFromDBByQuery(string call);
        Task<TimedOutCall> InsertFailedCallInDB(string symbol, string? call, string type);
        Task<StockDataDTO?> RecallFailedCall(TimedOutCall call);
        void DeleteFailedCallInDB(TimedOutCall call);
    }
}
