using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Services;
using API.Settlement.Infrastructure.Services.SQLiteServices;
using Hangfire;
using System.Configuration;
using System.Data.SQLite;

namespace API.Settlement.Extensions
{
    public static class ServiceExtensions
	{
		public static void AddSQLiteConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<SQLiteConnection>(_ => new SQLiteConnection(configuration.GetConnectionString("SQLiteConnection")));
			services.AddSingleton<IDatabaseInitializer>(_ => new DatabaseInitializer(configuration.GetConnectionString("SQLiteConnection")));
		}
		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddAutoMapper(typeof(Infrastructure.Mappings.MappingProfile).Assembly);

			services.AddScoped<IFailedTransactionRepository, FailedTransactionRepository>();
			services.AddScoped<ISuccessfulTransactionRepository, SuccessfulTransactionRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<ITransactionResponseHandlerService, TransactionResponseHandlerService>();
			services.AddSingleton<IInfrastructureConstants, InfrastructureConstants>();
			services.AddTransient<ITransactionMapperService, TransactionMapperService>();
			services.AddTransient<IDateTimeService, DateTimeService>();
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();
			services.AddTransient<ITransactionWrapper, TransactionWrapper>();
			services.AddTransient<IJobService, JobService>();
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
				var _databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
				_databaseInitializer.Initialize();
			}
		}
	}
}