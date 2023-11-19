using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.Settings;
using API.Accounts.BackgroundServices;
using API.Accounts.Implementations;
using API.Accounts.Application;

namespace API.Accounts.Extensions
{
    public static class ServiceConfiguration
    {
        public static void AddAccountServicesConfiguration(this IServiceCollection services)
        {
            services
                .InjectData()
                .AddHttpClientServices()
                .AddSettings()
                .AddClockBackgroundService()
                .AddAccountServices()
                .AddAccountAuthentication();

        }

        public static IServiceCollection InjectData(this IServiceCollection services)
        {
            services.AddApplicationData();

            // Sql Db
            //services.UseSqlDatabase<AccountDataAdapter>();

            // mocking memory db
            services.UseMemoryMockupDb();

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientRoutes, HttpClientRoutes>();
            services.AddScoped<IHttpService, HttpServiceDecorator>();
            services.AddHttpClient();

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsSettingsManager, AccountSettingsAdapter>();

            return services;
        }

        public static IServiceCollection AddClockBackgroundService(this IServiceCollection services)
        {
            services.AddApplicationEventClock();
            services.AddHostedService<EventClockService>();
            services.AddHostedService<StartupSetupService>();
            return services;
        }
    }
}
