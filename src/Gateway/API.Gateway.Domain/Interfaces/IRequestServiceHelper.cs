using API.Gateway.Domain.Entities.MongoDBEntities;
using MongoDB.Driver;

namespace API.Gateway.Infrastructure.Helpers
{
	public interface IRequestServiceHelper
	{
		Task Create(Request request);
		Task<List<Request>?> FindByQuery(FilterDefinition<Request> query);
	}
}