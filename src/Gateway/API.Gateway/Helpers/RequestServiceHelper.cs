using API.Gateway.Domain.Entities.MongoDBEntities;
using API.Gateway.Infrastructure.Services.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace API.Gateway.Infrastructure.Helpers
{
	public class RequestServiceHelper : IRequestServiceHelper
	{
		private readonly IMongoCollection<Request> _requestCollection;

		public RequestServiceHelper(IOptions<MongoDBConfiguration> mongoDbSettings)
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
				Log.Information($"Error inserting data in mongoDB: {ex.Message}");
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
				Log.Information($"Error finding by query: {ex.Message}");

				return null;
			}
		}
	}
}
