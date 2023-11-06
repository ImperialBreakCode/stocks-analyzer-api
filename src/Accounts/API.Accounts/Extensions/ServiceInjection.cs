using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.Services.HttpService;
using API.Accounts.Application.Services.StockService;
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

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsSettingsManager, AccountSettingsAdapter>();
            
            services.AddSingleton<IHttpClientRoutes, HttpClientRoutes>();
            services.AddScoped<IHttpService, HttpServiceDecorator>();
            services.AddHttpClient();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IStockActionManager, StockActionManager>();
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
    }
}
