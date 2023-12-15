using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces.Services
{
    public interface IWebSocketService
    {
        Task ProcessWebSocketRequest(HttpContext httpContext);
    }
}
