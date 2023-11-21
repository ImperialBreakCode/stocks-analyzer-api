using System.Net;

namespace API.Settlement.Extensions.Middlewares
{
	public class IPFilterMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly HashSet<string> _blackListedIPs;

		public IPFilterMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_blackListedIPs = new HashSet<string>(configuration.GetSection("BlackListedIPs").Get<List<string>>());
		}
		public async Task Invoke(HttpContext context)
		{
			var remoteIpAddress = context.Connection.RemoteIpAddress;
			string remoteIp = remoteIpAddress.IsIPv4MappedToIPv6 
				? remoteIpAddress.MapToIPv4().ToString()
				: remoteIpAddress.ToString();

			if (_blackListedIPs.Contains(remoteIp))
            {
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return;
            }

			await _next(context);
        }
	}
}
