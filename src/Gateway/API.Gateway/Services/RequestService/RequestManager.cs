using API.Gateway.Domain.Entities.Factories;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Helpers;
using Serilog;

namespace API.Gateway.Services.RequestService
{
	public class RequestManager : IRequestManager
	{
		private readonly RequestFactory _factory;
		private readonly IJwtTokenParser _jwtTokenParser;
		private readonly IRequestService _requestService;
		public RequestManager(RequestFactory requestFactory, IJwtTokenParser jwtTokenParser, IRequestService requestService)
		{
			_factory = requestFactory;
			_jwtTokenParser = jwtTokenParser;
			_requestService = requestService;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				DateTime dateTime = DateTime.UtcNow.AddHours(2);
				string? controller = context.GetRouteData().Values["controller"]?.ToString();
				string? ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
				string username = _jwtTokenParser.GetUsernameFromToken();
				string? route = context.GetRouteData().Values["action"]?.ToString();

				if (controller != "WebSocket")
				{
					var request = _factory.Create(dateTime, controller, ip, username, route);

					await _requestService.Create(request);
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error creating the request for the database: {ex.Message}");
			}
		}
	}
}
