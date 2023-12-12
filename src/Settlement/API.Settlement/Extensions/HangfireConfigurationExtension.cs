using Hangfire;

namespace API.Settlement.Extensions
{
    public static class HangfireConfigurationExtension
    {
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

	}
}