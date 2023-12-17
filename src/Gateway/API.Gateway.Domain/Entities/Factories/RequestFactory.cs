using API.Gateway.Domain.Entities.MongoDBEntities;

namespace API.Gateway.Domain.Entities.Factories
{
	public class RequestFactory
	{
		public Request Create(DateTime? dateTime, string? controller, string? ip, string? username, string? route)
		{
			return new Request
			{
				DateTime = dateTime,
				Controller = controller,
				Ip = ip,
				Username = username,
				Route = route
			};
		}
	}
}
