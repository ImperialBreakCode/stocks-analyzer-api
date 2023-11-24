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
using API.Accounts.Application.Settings.GatewayAuthSettingsSender;
using API.Accounts.Application.Settings.GatewaySettingsSender;
using API.Accounts.Application.Settings;
using API.Accounts.Application.Settings.UpdateHandlers;
using Microsoft.Extensions.DependencyInjection;
using API.Accounts.Application.Services.UserService.UserRankService;
using API.Accounts.Application.Services.UserService.EmailService;

namespace API.Accounts.Application
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationData(this IServiceCollection services)
        {
            services.AddTransient<IStocksData, StocksDataMockup>();
            services.AddTransient<IExchangeRatesData, ExchangeRateDataMockup>();
            return services;
        }

        public static IServiceCollection UseSqlDatabase<TDataAdapter>(this IServiceCollection services) 
            where TDataAdapter : class, IAccountsData
        {
            services.AddTransient<ISqlContextCreator, SqlContextCreator>();
            services.AddTransient<IAccountsData, TDataAdapter>();
            return services;
        }

        public static IServiceCollection UseMemoryMockupDb(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsData, AccountMockupData>();
            return services;
        }

        public static IServiceCollection AddHttpClientServices<T>(this IServiceCollection services)
            where T : class, IHttpService
        {
            services.AddSingleton<IHttpClientRoutes, HttpClientRoutes>();
            services.AddScoped<IHttpService, T>();

            return services;
        }

        public static IServiceCollection AddAccountAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<ITokenManager, TokenManager>();

            return services;
        }

        public static IServiceCollection AddApplicationEventClock(this IServiceCollection services)
        {
            services.AddSingleton<IEventClock, EventClock>();
            services.AddSingleton<IDemoWalletDeleteHandler, DemoWalletDeleteHandler>();
            services.AddSingleton<IAuthTokenGatewayNotifyer, AuthTokenGatewayNotifyer>();

            return services;
        }

        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            AddStockService(services);
            AddUserService(services);

            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }

        public static IServiceCollection AddSettings<TSettingsAdapter>(this IServiceCollection services)
            where TSettingsAdapter : class, IAccountsSettingsManager
        {
            services.AddSingleton<IAccountsSettingsManager, TSettingsAdapter>();
            services.AddTransient<ISocketGatewaySettingsSender, SocketGatewaySettingsSender>();

            return services;
        }

        private static void AddStockService(IServiceCollection services)
        {
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IStockActionExecuter, StockActionExecuter>();
            services.AddTransient<IStockActionFinalizer, StockActionFinalizer>();
            services.AddTransient<IStockActionManager, StockActionManager>();
        }

        private static void AddUserService(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRankManager, UserRankManager>();
            services.AddTransient<IEmailConfirmation, EmailConfirmation>();
        }
    }
}
