using API.Settlement.Domain.Entities;
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
			services.AddSingleton<IDictionary<string, ICollection<Wallet>>>(new Dictionary<string, ICollection<Wallet>>());
			services.AddSingleton<IUserWalletDictionaryService, UserWalletDictionaryService>();
			services.AddTransient<IJobService, JobService>();
			services.AddSingleton<IHangfireService, HangfireService>();
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();
			services.AddScoped<ISettlementServiceWrapper, SettlementServiceWrapper>();
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