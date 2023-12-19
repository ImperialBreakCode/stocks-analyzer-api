using API.Gateway.Domain.Entities.MongoDBEntities;

namespace API.Gateway.Domain.Interfaces
{
	public interface IRequestService
	{
		Task Create(Request request);
		Task<string> GetRouteStatisticsLast24Hours(string route);
		Task<string> GetRouteStatisticsLast24Hours();
	}
}
