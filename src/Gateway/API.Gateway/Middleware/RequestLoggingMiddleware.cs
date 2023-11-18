using Serilog;
using System.Text;

namespace API.Gateway.Middleware
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
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

			Log.Information("Response: {StatusCode}", context.Response.StatusCode);

			using (var responseBodyStream = new MemoryStream())
			{
				var originalResponseBody = context.Response.Body;
				context.Response.Body = responseBodyStream;

				await _next(context);

				responseBodyStream.Seek(0, SeekOrigin.Begin);
				using (var reader = new StreamReader(responseBodyStream))
				{
					var responseBody = await reader.ReadToEndAsync();
					Log.Information("Response Body: {ResponseBody}", FormatJsonHelper.FormatJson(responseBody));
				}

				context.Response.Body = originalResponseBody;
			}

		}
	}
}
