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
        Task<IActionResult> Post(string url, object obj);
        Task<IActionResult> Get(string url);
        Task<IActionResult> Put(string url, object obj);
        Task<IActionResult> Delete(string url);
    }
}
