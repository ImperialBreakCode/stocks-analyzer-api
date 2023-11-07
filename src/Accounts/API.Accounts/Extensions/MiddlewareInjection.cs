using API.Accounts.Middleware;

namespace API.Accounts.Extensions
{
    public static class MiddlewareInjection
    {
        public static void UseAccountMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<HostWhitelistGuard>();
        }
    }
}
