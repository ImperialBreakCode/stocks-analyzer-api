using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Services;
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
		private readonly IWebSocketHandler _webSocketService;

		public WebSocketController(IWebSocketHandler webSocketService)
		{
			_webSocketService = webSocketService;
		}

		[HttpGet("/ws")]
		public async Task Get()
		{
			await _webSocketService.ProcessWebSocketRequest(HttpContext);
		}
	}
}
