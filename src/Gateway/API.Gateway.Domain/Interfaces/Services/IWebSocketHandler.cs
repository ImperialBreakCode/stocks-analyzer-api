using Microsoft.AspNetCore.Http;

namespace API.Gateway.Domain.Interfaces.Services
{
	public interface IWebSocketHandler
	{
		Task ProcessWebSocketRequest(HttpContext httpContext);
	}
}
