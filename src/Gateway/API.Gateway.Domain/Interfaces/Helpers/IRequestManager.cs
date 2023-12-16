using Microsoft.AspNetCore.Http;

namespace API.Gateway.Domain.Interfaces.Helpers
{
    public interface IRequestManager
    {
        Task Invoke(HttpContext context);
    }
}