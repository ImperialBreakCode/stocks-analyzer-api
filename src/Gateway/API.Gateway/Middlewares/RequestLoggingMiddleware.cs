using API.Gateway.Domain.Entities.MongoDBEntities;
using API.Gateway.Domain.Interfaces;
using Serilog;
using System.Text;

namespace API.Gateway.Middleware
{
    public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IJwtTokenParser _jwtTokenParser;
		private readonly IRequestService _requestService;

		public RequestLoggingMiddleware(RequestDelegate next, IJwtTokenParser jwtTokenParser, IRequestService requestService)
		{
			_next = next;
			_jwtTokenParser = jwtTokenParser;
			_requestService = requestService;
		}

		public async Task Invoke(HttpContext context)
		{
			DateTime dateTime = DateTime.UtcNow.AddHours(2);
			string controller = context.GetRouteData().Values["controller"]?.ToString();
			string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
			string username = _jwtTokenParser.GetUsernameFromToken();
			string route = context.GetRouteData().Values["action"]?.ToString();


			Request request = new Request
			{
				DateTime = dateTime,
				Controller = controller,
				Ip = ip,
				Username = username,
				Route = route
			};

			_requestService.Create(request);

			Log.Information("=============================================================================================");
			Log.Information("Incoming Request: {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);

			if (context.Request.ContentLength > 0)
			{
				context.Request.EnableBuffering();
				using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
				{
					var requestBody = await reader.ReadToEndAsync();
					Log.Information("Request Body: {RequestBody}", requestBody);
					context.Request.Body.Seek(0, SeekOrigin.Begin);
				}
			}

			var userId = context.User.Identity.Name;
			if (userId != null)
			{
				Log.Information("Request from User: {UserId}", userId);
			}


			using (var responseBodyStream = new MemoryStream())
			{
				var originalResponseBody = context.Response.Body;
				context.Response.Body = responseBodyStream;

				await _next(context);

				responseBodyStream.Seek(0, SeekOrigin.Begin);
				using (var reader = new StreamReader(responseBodyStream))
				{
					Log.Information("Incoming Response:");
					var responseBody = await reader.ReadToEndAsync();
					if (responseBody.Length > 0)
					{
						Log.Information("Response Body: {ResponseBody}", FormatJsonHelper.FormatJson(responseBody));
					}
				}

				context.Response.Body = originalResponseBody;
			}

		}
	}
}
