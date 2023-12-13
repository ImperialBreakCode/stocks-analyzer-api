using API.Gateway.Domain.Entities.MongoDBEntities;
using MongoDB.Driver;

namespace API.Gateway.Domain.Interfaces
{
    public interface IRequestServiceHelper
    {
        Task Create(Request request);
        Task<List<Request>?> FindByQuery(FilterDefinition<Request> query);
    }
}