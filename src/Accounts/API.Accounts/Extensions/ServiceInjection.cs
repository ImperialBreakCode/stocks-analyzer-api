using API.Accounts.Application.Data;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Data;

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
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
