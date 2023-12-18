using API.Gateway.Domain.Entities.Factories;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Helpers;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Helpers;
using API.Gateway.Infrastructure.Contexts;
using API.Gateway.Infrastructure.Init;
using API.Gateway.Infrastructure.Repositories;
using API.Gateway.Services;

namespace API.Gateway.DataInjection
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddMyHttpClient(this IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IHttpClient, GwHttpClient>();

			return services;
		}

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IStockInfoService, StockInfoService>();
			services.AddTransient<IAnalyzerService, AnalyzerService>();
			services.AddTransient<IWalletService, WalletService>();
			services.AddTransient<IStockService, StockService>();
			services.AddTransient<IRequestService, RequestService>();

			services.AddTransient<IWebSocketHandler, WebSocketHandler>();

			return services;
		}

		public static IServiceCollection AddHelpers(this IServiceCollection services)
		{
			services.AddSingleton<IJwtTokenParser, JwtTokenParser>();
			services.AddTransient<ICacheHelper, CacheHelper>();

			return services;
		}

		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddTransient<IEmailRepository, EmailRepository>();
			services.AddTransient<IRequestRepository, RequestRepository>();

			return services;
		}

		public static IServiceCollection AddFacoties(this IServiceCollection services)
		{
			services.AddTransient<ResponseDTOFactory>();
			services.AddTransient<RequestFactory>();

			return services;
		}

		public static IServiceCollection AddDbHelpers(this IServiceCollection services)
		{
			services.AddTransient<IDatabaseInit, SQLiteDBInit>();
			services.AddSingleton<SQLiteContext>();

			return services;
		}
	}
}
