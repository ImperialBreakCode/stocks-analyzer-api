using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Domain.Interfaces.Services
{
	public interface IStockInfoService
	{
		Task<IActionResult> GetCurrentData(string companyName);
		Task<IActionResult> GetDailyData(string companyName);
		Task<IActionResult> GetWeeklyData(string companyName);
		Task<IActionResult> GetMonthlyData(string companyName);
	}
}
