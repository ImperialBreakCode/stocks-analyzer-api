using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.Data.StocksData;
using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.Services.StockService;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Application.Services.StockService.SubServices;
using API.Accounts.Application.Services.TransactionService;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Application.Services.WalletService;
using API.Accounts.Application.Settings;
using API.Accounts.Implementations;

namespace API.Accounts.Extensions
{
    public static class ServiceInjection
    {
        public static IServiceCollection InjectData(this IServiceCollection services)
        {
            services.AddTransient<ISqlContextCreator, SqlContextCreator>();
            services.AddTransient<IAccountsData, AccountDataAdapter>();
            services.AddTransient<IStocksData, StocksData>();

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsSettingsManager, AccountSettingsAdapter>();

            AddStockService(services);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }

        public static IServiceCollection InjectAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<ITokenManager, TokenManager>();

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientRoutes, HttpClientRoutes>();
            services.AddScoped<IHttpService, HttpServiceDecorator>();
            services.AddHttpClient();

            return services;
        }

        private static void AddStockService(IServiceCollection services)
        {
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IStockActionExecuter, StockActionExecuter>();
            services.AddTransient<IStockActionFinalizer, StockActionFinalizer>();
            services.AddTransient<IStockActionManager, StockActionManager>();
        }
    }
}
