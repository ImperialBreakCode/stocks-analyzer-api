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
                .AddApplicationData()
                .AddHttpClient()
                .AddHttpClientServices<HttpServiceDecorator>()
                .AddSettings<AccountSettingsAdapter>()
                .AddBackgroundServices()
                .AddAccountServices()
                .AddAccountAuthentication()
                .AddRabbitMQServices();
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddApplicationEventClock();
            services.AddHostedService<StartupService>();
            services.AddHostedService<EventClockService>();
            return services;
        }
    }
}
