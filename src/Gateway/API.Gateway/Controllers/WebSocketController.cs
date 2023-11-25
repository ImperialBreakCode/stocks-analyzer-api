using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;

namespace API.Gateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebSocketController : Controller
	{
		private readonly IWritableOptions<JwtOptionsConfiguration> _options;
		public WebSocketController(IWritableOptions<JwtOptionsConfiguration> options)
        {
            _options = options;
        }
        [HttpGet("/ws")]
		public async Task Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				var buffer = new byte[1024 * 4];
				using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				var receiveResult = await webSocket.ReceiveAsync(
				new ArraySegment<byte>(buffer), CancellationToken.None);

				var jsonData = Encoding.UTF8.GetString(buffer);
				AuthValues values = JsonConvert.DeserializeObject<AuthValues>(jsonData);

				_options.Value.Issuer = values.Issuer;
				_options.Value.Audience = values.Audience;
				_options.Value.SigningKey = values.SecretKey;

				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
			}
			else
			{
				HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}
	}
}
