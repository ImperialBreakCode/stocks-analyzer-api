using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces.Helpers;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Settings;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace API.Gateway.Services
{
    public class WebSocketService : IWebSocketService
	{
		private readonly IWritableOptions<JwtOptionsConfiguration> _options;

		public WebSocketService(IWritableOptions<JwtOptionsConfiguration> options)
		{
			_options = options;
		}

		public async Task ProcessWebSocketRequest(HttpContext httpContext)
		{
			if (httpContext.WebSockets.IsWebSocketRequest)
			{
				var buffer = new byte[1024 * 4];
				using var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
				var receiveResult = await webSocket.ReceiveAsync(
					new ArraySegment<byte>(buffer), CancellationToken.None);

				var jsonData = Encoding.UTF8.GetString(buffer);
				AuthValues values = JsonConvert.DeserializeObject<AuthValues>(jsonData);

				_options.Update(opt =>
				{
					opt.Issuer = values.Issuer;
					opt.Audience = values.Audience;
					opt.SigningKey = values.SecretKey;
				});

				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
			}
			else
			{
				httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}
	}
}
