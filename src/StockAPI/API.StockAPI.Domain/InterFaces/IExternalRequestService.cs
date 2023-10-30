using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.InterFaces
{
    public interface IExternalRequestService
    {
        string QueryStringGenerator(string symbol, string function);

        Task<string> JsonDataGenerator(string query);
    }
}
