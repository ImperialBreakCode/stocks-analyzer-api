using API.Gateway.Domain.Entities.MongoDBEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace API.Gateway.Infrastructure.Repositories
{
	public class RequestRepository : IRequestRepository
	{
		private readonly IMongoCollection<Request> _requestCollection;

		public RequestRepository(IOptions<MongoDBConfiguration> mongoDbSettings)
		{
			MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
			IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
			_requestCollection = database.GetCollection<Request>(mongoDbSettings.Value.CollectionName);
		}

		public async Task Create(Request request)
		{
			try
			{
				await _requestCollection.InsertOneAsync(request); ;
			}
			catch (Exception ex)
			{
				Log.Error($"Error inserting data in mongoDB: {ex.Message}");
			}
		}

		public async Task<List<Request>?> FindByQuery(FilterDefinition<Request> query)
		{
			try
			{
				var requests = await _requestCollection.Find(query).ToListAsync();

				return requests;
			}
			catch (Exception ex)
			{
				Log.Error($"Error finding by query: {ex.Message}");

				return null;
			}
		}
	}
}
