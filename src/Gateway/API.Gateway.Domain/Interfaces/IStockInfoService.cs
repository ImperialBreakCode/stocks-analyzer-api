﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IStockInfoService
	{
		Task<IActionResult> GetCurrentData(string companyName);
		Task<IActionResult> GetDailyData(string companyName);
		Task<IActionResult> GetWeeklyData(string companyName);
		Task<IActionResult> GetMonthlyData(string companyName);
	}	
}