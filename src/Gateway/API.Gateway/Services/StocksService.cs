﻿using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Services
{
	public class StocksService : IStocksService
	{
		private IHttpClient _httpClient;
		public StocksService(IHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		[HttpGet]
		public async Task<IActionResult> GetStockData(string dataType, string companyName)
		{
			var res = await _httpClient.GetActionResult($"api/Stock/{dataType}/{companyName}");
			return res;
		}
	}
}
