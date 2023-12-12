using API.Settlement.Middlewares;

namespace API.Settlement.Extensions
{
	public static class MiddlewareConfigurationExtension
	{
		public static void UseCustomMiddlewares(this IApplicationBuilder app) => AddIPFilteringMiddleware(app);

		private static void AddIPFilteringMiddleware(this IApplicationBuilder app) => app.UseMiddleware<IPFilteringMiddleware>();

	}
}
