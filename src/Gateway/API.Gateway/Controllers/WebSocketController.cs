using API.Gateway.Domain.DTOs;
using API.Gateway.Settings;
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
		private readonly IOptionsMonitor<JwtOptionsConfiguration> _optionsMonitor;
        public WebSocketController(IOptionsMonitor<JwtOptionsConfiguration> monitor)
        {
            _optionsMonitor = monitor;
        }
        [Route("/ws")]
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

				_optionsMonitor.CurrentValue.Issuer = values.Issuer;
				_optionsMonitor.CurrentValue.Audience = values.Audience;
				_optionsMonitor.CurrentValue.SigningKey = values.SecretKey;

				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
			}
			else
			{
				HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}
	}
}
