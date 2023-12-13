using API.Gateway.Domain.Entities.MongoDBEntities;
using API.Gateway.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace API.Gateway.Infrastructure.Services.MongoDB
{
    public class RequestService : IRequestService
	{
		private readonly IRequestServiceHelper _helper;

		public RequestService(IRequestServiceHelper helper)
		{
			_helper = helper;
		}

		public async Task Create(Request request)
		{
			_helper.Create(request);
		}

		public async Task<string> GetRouteStatisticsLast24Hours(string route)
		{
			var filter = Builders<Request>.Filter;
			var query = filter.Gte(r => r.DateTime, DateTime.UtcNow.AddDays(-1))
						 & filter.Eq(r => r.Route, route);

			var requests = await _helper.FindByQuery(query);
			if (requests == null)
			{
				return "There wasn an error fetching the required data from the database. Please try again.";
			}

			var totalLoggedInUserRequests = requests.Where(r => !string.IsNullOrEmpty(r.Username));

			var usernameCounts = totalLoggedInUserRequests.GroupBy(r => r.Username)
										 .Select(g => new { Username = g.Key, Count = g.Count() })
										 .OrderByDescending(g => g.Count);

			var mostFrequentUsername = usernameCounts.FirstOrDefault()?.Username ?? "No data found";
			var totalRequests = requests.Count();

			string answer = $"The number of requests made to this route in the last 24 hours is {totalRequests}. " +
							 $"The user who has made the most requests is '{mostFrequentUsername}'.";

			return answer;
		}

		public async Task<string> GetRouteStatisticsLast24Hours()
		{
			var filter = Builders<Request>.Filter;
			var query = filter.Gte(r => r.DateTime, DateTime.UtcNow.AddDays(-1));

			var requests = await _helper.FindByQuery(query);
			if (requests == null)
			{
				return "There wasn an error fetching the required data from the database. Please try again.";
			}

			var totalLoggedInUserRequests = requests.Where(r => !string.IsNullOrEmpty(r.Username));

			var usernameCounts = totalLoggedInUserRequests.GroupBy(r => r.Username)
										 .Select(g => new { Username = g.Key, Count = g.Count() })
										 .OrderByDescending(g => g.Count);

			var mostFrequentUsername = usernameCounts.FirstOrDefault()?.Username ?? "No data found";
			var mostFrequentUsernameRequests = totalLoggedInUserRequests.Where(r => r.Username == mostFrequentUsername).Count();

			var routeCounts = requests.GroupBy(r => r.Route)
									 .Select(g => new { Route = g.Key, Count = g.Count() })
									 .OrderByDescending(g => g.Count);

			var mostUsedRoute = routeCounts.FirstOrDefault()?.Route ?? "No data found";
			var requestsInMostUsedRoute = routeCounts.FirstOrDefault()?.Count ?? 0;

			var hourCounts = requests.Where(r => r.DateTime.HasValue)
									 .GroupBy(r => r.DateTime.Value.Hour)
									 .Select(g => new { Hour = g.Key, Count = g.Count() })
									 .OrderByDescending(g => g.Count);

			var mostUsedHour = hourCounts.FirstOrDefault()?.Hour ?? -1;
			var requestsInMostUsedHour = hourCounts.FirstOrDefault()?.Count ?? 0;

			string answer = $"The number of requests made to the API in the past 24 hours is {requests.Count}. " +
							 $"The user who has made the most requests is '{mostFrequentUsername}' with {mostFrequentUsernameRequests} requests. " +
							 $"The hour with the most usage was between '{mostUsedHour}' and '{mostUsedHour + 1}', with {requestsInMostUsedHour} requests. " +
							 $"The most used route was '{mostUsedRoute}', with {requestsInMostUsedRoute} requests.";

			return answer;
		}
	}
}
