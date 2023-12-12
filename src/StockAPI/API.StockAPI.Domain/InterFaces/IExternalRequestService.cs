using API.StockAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.InterFaces
{
    public interface IExternalRequestService
    {
        string QueryStringGenerator(string symbol, string type);
        Task<HttpResponseMessage?> ExecuteQuery(string symbol, string query, string type);
        Task<string?> GetDataFromQuery(HttpResponseMessage response);
    }
}
