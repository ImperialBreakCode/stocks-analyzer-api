using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IHttpClient
	{
		Task<string> PostAsync(string url, string message);
		Task<IActionResult> PostAsJsonAsync(string url, object obj);
		Task<string> PostAsJsonAsyncReturnString(string url, object obj);
		Task<string> GetStringAsync(string url);
		Task<IActionResult> GetActionResult(string url);
		Task<IActionResult> PostActionResult(string url, string message);
		Task<IActionResult> PutActionResult(string url, object obj);
	}
}
