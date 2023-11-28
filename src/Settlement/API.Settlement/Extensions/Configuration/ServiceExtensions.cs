using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.MongoDb.WalletDb;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Services;
using API.Settlement.Infrastructure.Services.SQLiteServices;
using API.Settlement.Infrastructure.Services.MongoDbServices.WalletDbServices;
using Hangfire;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data.SQLite;
using API.Settlement.Infrastructure.Services.EmailServices;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Infrastructure.Services.SQLiteServices.TransactionDatabase;
using API.Settlement.Extensions.Middlewares;

namespace API.Settlement.Extensions.Configuration
{
    public static class ServiceExtensions
    {
        public static void AddSQLiteTransactionDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ => new SQLiteConnection(configuration.GetConnectionString("SQLiteConnection")));
            services.AddSingleton<ISQLiteTransactionDatabaseInitializer>(_ => new SQLiteTransactionDatabaseInitializer(configuration.GetConnectionString("SQLiteConnection")));
        }
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddAutoMapper(typeof(Infrastructure.Mappings.MappingProfile).Assembly);

            services.AddScoped<IUserCommissionService, UserCommissionService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IFailedTransactionRepository, FailedTransactionRepository>();
            services.AddScoped<ISuccessfulTransactionRepository, SuccessfulTransactionRepository>();
            services.AddScoped<ITransactionDatabaseContext, TransactionDatabaseContext>();
            services.AddTransient<ITransactionResponseHandlerService, TransactionResponseHandlerService>();
            services.AddTransient<IInfrastructureConstants, InfrastructureConstants>();
            services.AddTransient<ITransactionMapperService, TransactionMapperService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IBuyService, BuyService>();
            services.AddTransient<ISellService, SellService>();
            services.AddTransient<ITransactionWrapper, TransactionWrapper>();
            services.AddTransient<IHangfireJobService, HangfireJobService>();
            services.AddScoped<IHangfireService, HangfireService>();
            services.AddScoped<ISettlementService, SettlementService>();
        }
        public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            services.AddHangfireServer();
        }
        public static void UseDatabaseInitialization(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _databaseInitializer = scope.ServiceProvider.GetRequiredService<ISQLiteTransactionDatabaseInitializer>();
                _databaseInitializer.Initialize();
            }
        }

        public static void AddWalletDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WalletDatabaseSettings>(
                configuration.GetSection(nameof(WalletDatabaseSettings)));

            services.AddSingleton<IWalletDatabaseSettings>(w =>
                w.GetRequiredService<IOptions<WalletDatabaseSettings>>().Value);

            services.AddSingleton<IMongoClient>(m =>
                new MongoClient(configuration.GetValue<string>("WalletDatabaseSettings:ConnectionString")));

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletService, WalletService>();
        }

        public static void UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<IPFilterMiddleware>();
        }
    }
}