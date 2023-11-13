using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.Data.ExchangeRates;
using API.Accounts.Application.Data.StocksData;
using API.Accounts.Application.EventClocks;
using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.Services.StockService;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Application.Services.StockService.SubServices;
using API.Accounts.Application.Services.TransactionService;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Application.Services.WalletService;
using API.Accounts.Application.Settings;
using API.Accounts.BackgroundServices;
using API.Accounts.Implementations;

namespace API.Accounts.Extensions
{
    public static class ServiceConfiguration
    {
        public static void AddAccountServicesConfiguration(this IServiceCollection services)
        {
            services
                .InjectData()
                .AddHttpClientServices()
                .AddAuthentication()
                .AddSettings()
                .AddServices();
        }

        public static IServiceCollection InjectData(this IServiceCollection services)
        {
            services.AddTransient<ISqlContextCreator, SqlContextCreator>();
            services.AddTransient<IAccountsData, AccountDataAdapter>();
            services.AddTransient<IStocksData, StocksDataMockup>();
            services.AddTransient<IExchangeRatesData, ExchangeRateDataMockup>();

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientRoutes, HttpClientRoutes>();
            services.AddScoped<IHttpService, HttpServiceDecorator>();
            services.AddHttpClient();

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<ITokenManager, TokenManager>();

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsSettingsManager, AccountSettingsAdapter>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            AddEventClock(services);
            AddStockService(services);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }

        private static void AddStockService(IServiceCollection services)
        {
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IStockActionExecuter, StockActionExecuter>();
            services.AddTransient<IStockActionFinalizer, StockActionFinalizer>();
            services.AddTransient<IStockActionManager, StockActionManager>();
        }

        private static void AddEventClock(IServiceCollection services)
        {
            services.AddSingleton<IEventClock, EventClock>();
            services.AddSingleton<IDemoWalletDeleteHandler, DemoWalletDeleteHandler>();
            services.AddHostedService<EventClockService>();
        }
    }
}
