using API.Accounts.Application.Settings;

namespace API.Accounts.Middleware
{
    public class HostWhitelistGuard
    {

        private RequestDelegate _next;

        public HostWhitelistGuard(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAccountsSettingsManager settingsManager)
        {
            //string ip = $"{httpContext.Connection.RemoteIpAddress}:{httpContext.Connection.RemotePort}";
            string ip = $"{httpContext.Connection.RemoteIpAddress}";

            if (settingsManager.AllowedHosts.Contains(ip))
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsync("Access Denied");
            }
        }
    }
}
