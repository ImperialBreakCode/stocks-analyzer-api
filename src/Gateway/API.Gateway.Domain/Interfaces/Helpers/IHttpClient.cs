using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Domain.Interfaces
{
	public interface IHttpClient
    {
        Task<IActionResult> Post(string url, object obj);
        Task<IActionResult> Get(string url);
        Task<IActionResult> Put(string url, object obj);
        Task<IActionResult> Delete(string url);
    }
}
