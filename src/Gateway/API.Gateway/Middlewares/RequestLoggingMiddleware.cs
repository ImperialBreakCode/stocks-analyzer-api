using API.Gateway.Domain.Entities.MongoDBEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Helpers;
using Serilog;
using System.Text;

namespace API.Gateway.Middleware
{
    public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IRequestManager _requestManager;

		public RequestLoggingMiddleware(RequestDelegate next, IRequestManager requestManager)
		{
			_next = next;
			_requestManager = requestManager;
		}

		public async Task Invoke(HttpContext context)
		{
			await _requestManager.Invoke(context);

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
					responseBodyStream.Seek(0, SeekOrigin.Begin);
					await responseBodyStream.CopyToAsync(originalResponseBody);
				}
				context.Response.Body = originalResponseBody;
			}
		}
	}
}
