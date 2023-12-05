using API.Gateway.Domain.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
    public interface IRequestService
	{
		Task Create(Request request);
		Task<string> GetRouteStatisticsLast24Hours(string route);
		Task<string> GetRouteStatisticsLast24Hours();
	}
}
