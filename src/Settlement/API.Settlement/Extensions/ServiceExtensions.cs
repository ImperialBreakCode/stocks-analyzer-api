using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Services;
using Hangfire;

namespace API.Settlement.Extensions
{
	public static class ServiceExtensions
	{
		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddAutoMapper(typeof(Infrastructure.Mappings.MappingProfile).Assembly);
			services.AddSingleton<IInfrastructureConstants, InfrastructureConstants>();
			services.AddTransient<ITransactionMapperService, TransactionMapperService>();
			services.AddTransient<IDateTimeService, DateTimeService>();
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();
			services.AddTransient<ITransactionWrapper, TransactionWrapper>();
			services.AddTransient<IJobService, JobService>();
			services.AddSingleton<IHangfireService, HangfireService>();
			services.AddScoped<ISettlementService, SettlementService>();
		}
		public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHangfire(config => config
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

			services.AddHangfireServer();
		}
	}
}